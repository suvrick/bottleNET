using bottlelib.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace bottlelib
{
    public class Core
    {
        //https://bottle4.realcdn.ru/www/content.json

        private int _indexPacket = 0;
        private ClientWebSocket socket;
        private bool _gameOver;
        private Player _player;
        private int _count = 0;
        private IClientPacket _packet;
        private bool _noHistory = false;
        private int _delayTime = 500;



        public async Task<Player> Go(Player player, IClientPacket packet = null, int count = 1)
        {

            _indexPacket = 0;
            _packet = packet;
            _count = count;
            _player = player;
            _gameOver = false;

            try
            {
                using (socket = new ClientWebSocket())
                {
                    await socket.ConnectAsync(new Uri(GameSettings.HOST), CancellationToken.None);

                    var authPack = _player.Url.ToAuth().ToPack();
                    await SendAsync(authPack);

                    byte[] buffer;
                    int buffer_len = 1024;

                    while (socket.State == WebSocketState.Open && !_gameOver)
                    {
                        buffer = new byte[buffer_len];
                        var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        //if (!result.EndOfMessage)
                        //{
                        //    buffer_len = BitConverter.ToInt32(buffer);
                        //    continue;
                        //}264

                        var buffer2 = new byte[buffer.Length];
                        short type = BitConverter.ToInt16(buffer, 8);
                        Array.Copy(buffer, 10, buffer2, 0, buffer2.Length - 10);

                        _gameOver = await ResponceParse(type, buffer2);
                        //buffer_len = 4;
                    }

                    await Task.Delay(_delayTime);
                }
            }
            catch (Exception ex)
            {
                _player.AuthResult = AuthResultType.ERROR_APP.ToString();
                if (ex.Message.Contains("status code '502'"))
                {
                    _player.AuthResult = AuthResultType.UPDATE_GAME.ToString();
                }

                Console.WriteLine($"[Go] Socket exception: {ex.Message}");
            }
            finally
            {
                Close();
            }

            return _player;
        }


        private async Task<bool> ResponceParse(short type, byte[] buffer)
        {
            switch (type)
            {
                case 4:
                    {
                        var field = new ServerPacket.Login(buffer);

                        _player.AuthResult = field.status.ToString();

                        switch (field.status)
                        {
                            case AuthResultType.LOGIN_SUCCESS:
                                _player.InnerId = field.inner_id;
                                _player.Balance = field.balance;

                                if (_packet != null)
                                {
                                    var buf = _packet.ToPack();
                                    await SendAsync(buf, _count);


                                    if (_packet is PacketClient.Roll)
                                    {

                                    }
                                    else
                                    {
                                        _packet = null;
                                        _count = 1;
                                        var core2 = new Core();
                                        core2._noHistory = true;
                                        _player = await core2.Go(_player);
                                        core2._noHistory = false;
                                    }

                                }
                                break;
                            case AuthResultType.LOGIN_EXIST:
                                await SendAsync(_player.Url.ToAuth().ToPack());
                                break;
                            default:
                                return true;
                        }
                        break;
                    }
                case 5:
                    {
                        var field = new ServerPacket.Info(buffer);
                        _player.Name = field.name;
                        _player.Photo = field.avatar;
                        _player.Profile = field.profile;


                        if (_packet == null)
                            return true;

                        break;
                    }
                case 7:
                    {
                        var field = new ServerPacket.Balance(buffer);
                        _player.Balance = field.balance;
                        break;
                    }
                case 17:
                    {
                        var field = new ServerPacket.Bonus(buffer);
                        _player.IsBonus = field.can_collect != 0x00 ? true : false;
                        _player.Bonus = 0;

                        if (_player.IsBonus)
                        {
                            var pack = new PacketClient
                                            .BonusDaily()
                                            .ToPack();
                            await SendAsync(pack);
                            _player.Bonus = field.bonus;
                            _player.Balance += _player.Bonus;
                        }
                        break;
                    }
                case 13:
                    {
                        //Reading packet GAME_REWARDS:13 with id 104 and length 1 data: 1226,1,{"id":1226,"content":{"hearts":9},"captions":{"en":"Great shot! Your reward","ru":"Отличный выстрел! Твоя награда"},"max_count":10000,"type":"roulette"}

                        var field = new ServerPacket.Reward(buffer);

                        var pack2 = new PacketClient.Reward(field.id);
                        await SendAsync(pack2.ToPack());

                       // Console.WriteLine($"Roll {field.json}");
                        //Console.WriteLine("Get reward");
                        //Console.WriteLine(BitConverter.ToString(pack2.ToPack()));
                        _packet = null;
                        _count = 1;
                        var core2 = new Core();
                        core2._noHistory = true;
                        _player = await core2.Go(_player);
                        core2._noHistory = false;
                        return true;
                    }
                case 264:
                    {
                        //"I[BBBIIS]",						
                        // BANS(264); target_id:I, [type:B, reason:B, repeated:B, moderator_id:I, duration:I, link:S]
                        _player.AuthResult = AuthResultType.BAN.ToString();
                        return true;
                    }
                default:
                    break;
            }

            return false;
        }
        public async Task SendAsync(byte[] packet, int count = 1)
        {
            try
            {
                for (int i = 0; i < count; ++i)
                {
                    if (socket == null || socket.State != WebSocketState.Open)
                        return;

                    var index = BitConverter.GetBytes(packet.Length - 4);
                    packet[0] = index[0];
                    packet[1] = index[1];
                    packet[2] = index[2];
                    packet[3] = index[3];

                    var index2 = BitConverter.GetBytes(_indexPacket);
                    packet[4] = index2[0];
                    packet[5] = index2[1];
                    packet[6] = index2[2];
                    packet[7] = index2[3];

                    await socket.SendAsync(new ArraySegment<byte>(packet), WebSocketMessageType.Binary, true, CancellationToken.None);
                    _indexPacket++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SendAsync] : Error send pack ({ex.Message})");

            }

        }
        private void UpdateHistory()
        {
            if (_noHistory)
                return;

            var history = _player.History;
            if (string.IsNullOrEmpty(history))
                history = "";

            var historyArray = history.Split('|', StringSplitOptions.RemoveEmptyEntries);
            var len = historyArray.Length;

            if (len < 10)
            {
                Array.Resize(ref historyArray, 10);
            }

            var time = DateTime.Now;
            var status = _player.AuthResult;
            var balance = _player.Balance;
            var bonus = _player.Bonus;
            var isBonus = _player.IsBonus;
            var result = $"{time}~{status}~{balance}~{bonus}~{isBonus}";

            var arr = new string[10];
            arr[0] = result + "|";

            for (int i = 1; i < arr.Length; i++)
            {
                arr[i] = historyArray[i - 1] + "|";
            }


            var history2 = string.Empty;
            for (int i2 = 0; i2 < arr.Length; i2++)
            {
                history2 += arr[i2];
            }

            _player.IsSelected = false;
            _player.History = history2;
        }
        public void Close()
        {
            UpdateHistory();

            _gameOver = true;
            socket = null;

            //Console.WriteLine("[Go] End game");
        }
    }
}
