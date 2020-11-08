using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace bottlelib
{
    public static class Parser
    {       
        public static PacketClient.Login ToAuth(this string input)
        {
            var netType = input.GetSocialType();

            string[] strArray = Dictionarys.INPUT_QUERY_WORDS[netType];

            var netId = input.FindWord(strArray[0]);
            var authKey = input.FindWord(strArray[1]);
            var sessionKey = input.FindWord(strArray[2]);

            return new PacketClient.Login()
            {
                net_id = string.IsNullOrEmpty(netId) ? 0 : long.Parse(netId),
                net_type = netType,
                device_type = DeviceType.DEVICE_PC,
                auth_key = authKey,
                oauth = default(short),
                session_key = sessionKey,
                referrer = 0,
                tag = 0,
                appicationID = 0x00,
                timestamp = "",
                language = 0x00,
                utm_source = ""
            };
        }

        //["395993763", 10, 7, "86af7f8b3a199db98dec53d56238519e", 0, "d4243dac419aa1f2d63627c0696bdd4c8068cce1221acb2847205cad224f36afd1cfee0bc5bce98cf9345", 0, 30, 0, "", 0, "", 0]
        public static Player CreatePlayer(this string input)
        {
            var netType = input.GetSocialType();
            string[] strArray = Dictionarys.INPUT_QUERY_WORDS[netType];
            var netId = input.FindWord(strArray[0]);
            var token = input.FindWord(strArray[1]);
            var token2 = input.FindWord(strArray[2]);

            if (netId.Equals(""))
                netId = Guid.NewGuid().ToString().Split('-')[0];

            if (token.Equals(""))
                token = Guid.NewGuid().ToString().Split('-')[0];

            return new Player()
            {
                Url = input,
                Name = "No name",
                PlayerId = $"{netType}{netId}",
                Token = token,
                AuthResult = "Create"
            };
        }

        public static NetType GetSocialType(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return NetType.NN;

            foreach (KeyValuePair<NetType, string[]> keyValuePair in Dictionarys.INPUT_SOCIAL_WORDS)
            {
                NetType key = keyValuePair.Key;
                foreach (string str in keyValuePair.Value)
                {
                    if (input.Contains(str))
                        return key;
                }
            }
            return NetType.NN;
        }

        public static string FindWord(this string input, string pattern, char stopChar = '&')
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(pattern))
                return string.Empty;

            int num1 = input.IndexOf(pattern);
            if (num1 == -1)
                return string.Empty;

            int num2 = pattern.Length + 1;
            string str = input.Substring(num1 + num2);
            string empty = string.Empty;
            for (int index = 0; index < str.Length; ++index)
            {
                char ch = str[index];
                if ((int)ch != (int)stopChar)
                    empty += ch.ToString();
                else
                    break;
            }
            return empty;
        }
    }


    public static class BinaryReaderExtiontion
    {
        public static string ReadString2(this BinaryReader reader)
        {
            int str_len = reader.ReadInt16();
            var str_arr = reader.ReadBytes(str_len);
            var zero = reader.ReadByte();
            return Encoding.Default.GetString(str_arr);
        }

        public static void WriteString2(this BinaryWriter reader, string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                reader.Write(new byte[] { 0x00, 0x00 });
                return;
            }

            List<byte> byteList = new List<byte>();
            byte[] bytes = Encoding.Default.GetBytes(input);
            byteList.AddRange(BitConverter.GetBytes((short)bytes.Length));
            byteList.AddRange(bytes);

            reader.Write(byteList.ToArray());
            return;
        }
    }

}
