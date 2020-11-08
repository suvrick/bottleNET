using bottlelib.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace bottlelib
{
    public class PacketClient
    {
        public class Login : IClientPacket
        {
            public Login()
            {
            }
			//"LBBS,BSIIBSBSB",		
			// LOGIN(4); net_id:L, type:B, device:B, auth_key:S, oauth:B, session_key:S, 
			// referrer:I, tag:I, appicationID:B, timestamp:S, language:B, utm_source:S, sex:B
			/*
			b3 00 00 00 
			00 00 00 00
			04 00
			01 
			cb 27 43 4e 23 00 00 00 
			04 
			01 
			20 00 643132333932653533323665303663343266396335383664613934653238353300 
			00
			6b 00 2d732d376332382d7677347a3437322e584e3830323661555750352e6230367a3171373034366178777435353234327a77723630323763782e4e33303837617a2e4f323432343179747538302d3334767850323131336279737135313461324e787531316231364e30533200
			3c 4e 00 00 
			17 00 00 00 
			00
			00 00 00 
			00 
			00 00
			00
			*/

			public long net_id;				// int
            public NetType net_type;		// 1
            public DeviceType device_type;  // 6
            public string auth_key;			// ""
            public short oauth;				// 0
            public string session_key;		// ""
            public int referrer;			// 0   
            public int tag;					// 18  
            public byte appicationID;		// 0   
            public string timestamp;        // ""	
            public byte language;			// 0   
            public string utm_source;       // ""  
			public byte flag = 0x00;		// 0

            public byte[] ToPack()
            {

                using (var stream = new MemoryStream())
                {
                    using (var writer = new BinaryWriter(stream))
                    {
						try
						{
							writer.Write(0);
							writer.Write(0);
							writer.Write((short)4);

							writer.Write((byte)0);

							writer.Write(net_id);
							writer.Write((byte)net_type);
							writer.Write((byte)device_type);
							writer.WriteString2(auth_key);
							writer.Write(oauth);
							writer.WriteString2(session_key);
							writer.Write(referrer);
							writer.Write(tag);
							writer.Write(appicationID);
							writer.WriteString2(timestamp);
							writer.Write(language);
							writer.WriteString2(utm_source);
							writer.Write(flag);
							writer.Write(flag);
							writer.Write(flag);
						}
						catch (Exception) { }

                        return stream.ToArray();
                    }
                }
            }

        }

		public class BonusDaily : IClientPacket
		{
			//07 00 00 00 
			// 2e 00 00 00 
			// 3d 00 06
			public byte[] ToPack()
			{
				using (var stream = new MemoryStream())
				{
					using (var writer = new BinaryWriter(stream))
					{
						try
						{
							writer.Write(0);
							writer.Write(0);
							writer.Write((short)61);
							writer.Write((byte)0);
						}
						catch (Exception) { }
						return stream.ToArray();
					}
				}
			}

		}

		public class Buy : IClientPacket
		{
			public int good_id;			//категория 
			public int cost;			//цена
			public int target_id;		//кому
			public int data;			// 0

			public string hash;
			public string _params;
			/// [65, 10, 43683275, 10086, 0, "a1f336c144bcedd5f58d6fa45ceaec30", "{"category": 71, "screen": 0}"]
			////[2, 2, 41803611, 9807, 0, "cfb8389b29e1d876ba7ae00ef591f434", "{"category": 70, "screen": 0}"]
			public byte[] ToPack()
			{
				using (var stream = new MemoryStream())
				{
					using (var writer = new BinaryWriter(stream))
					{
						try
						{
							writer.Write(0);
							writer.Write(0);
							writer.Write((short)6);

							writer.Write((byte)0);

							writer.Write(good_id);
							writer.Write(cost);
							writer.Write(target_id);
							writer.Write(data);

							writer.Write((byte)0);
							
							
							if (!string.IsNullOrEmpty(hash))
							{
								writer.WriteString2(hash);
								writer.Write((byte)0);
								writer.WriteString2(_params);
								writer.Write((byte)0);
							}
							
						}
						catch (Exception) { }


						//Console.WriteLine(stream.ToArray());
						return stream.ToArray();
					}
				}
			}

		}

		public class AddPhoto : IClientPacket
		{
			//	"BS,B",             
			// PHOTOS_ADD_PHOTO(204); photo_id:B, photo_url:S, active_id:B
			/*
				for( var i = 0; i < 20000; i++) {
					Main.connection.sendData(204, 3, "https://bottle2.itsrealgames.com/saved/143/6451126/1_1593935552.jpg");
					Main.connection.sendData(207,6451126, 3, true);
				}

				Main.connection.sendData(115,51119);
			*/

			public string photo_url { get; set; }
			public byte photo_id { get; set; }
			private byte active_id { get; set; } = 0;

			public byte[] ToPack()
			{
				using (var stream = new MemoryStream())
				{
					using (var writer = new BinaryWriter(stream))
					{
						try
						{
							writer.Write(0);
							writer.Write(0);
							writer.Write((short)204);
							writer.Write((byte)6);
							writer.Write(photo_id);
							writer.WriteString2(photo_url);
							writer.Write(active_id);
						}
						catch (Exception) { }
						return stream.ToArray();
					}
				}
			}
		}

		public class Like : IClientPacket
		{
			/*
				for( var i = 0; i < 20000; i++) {
					Main.connection.sendData(204, 3, "https://bottle2.itsrealgames.com/saved/143/6451126/1_1593935552.jpg");
					Main.connection.sendData(207,6451126, 3, true);
				}

				Main.connection.sendData(115,51119);
			*/

			//	"IBB",              
			// PHOTOS_LIKE(207); player_id:I, photo_id:B, is_like:B

			public int targetId { get; set; }
			public byte photoId { get; set; }

			public byte[] ToPack()
			{
				using (var stream = new MemoryStream())
				{
					using (var writer = new BinaryWriter(stream))
					{
						try
						{
							writer.Write(0);
							writer.Write(0);
							writer.Write((short)207);
							writer.Write((byte)6);
							writer.Write(targetId);
							writer.Write(photoId);
							writer.Write((byte)1);
						}
						catch (Exception) { }
						return stream.ToArray();
					}
				}
			}
		}




		public class Roll : IClientPacket
		{
			//0700 0000 2f00 0000 fc00 06
			//Reading packet GAME_REWARDS:13 with id 115 and length 1 data: 1210,1,{"id":1210,"content":{"videos":1},"captions":{"en":"Great shot! Your reward","ru":"Отличный выстрел! Твоя награда"},"max_count":10000,"type":"roulette"}
			//Send packet ROULETTE_ROLL type 252 deviceType 6 id 47 data: 252

			public byte[] ToPack()
			{
				using (var stream = new MemoryStream())
				{
					using (var writer = new BinaryWriter(stream))
					{
						try
						{
							writer.Write(0);
							writer.Write(0);
							writer.Write((short)252);
							writer.Write((byte)6);
						}
						catch (Exception) { }
						return stream.ToArray();
					}
				}
			}
		}

		public class Reward : IClientPacket
		{
			public Reward(int _id)
			{
				id = _id;
			}

			public int id;
			public byte[] ToPack()
			{
				using (var stream = new MemoryStream())
				{
					using (var writer = new BinaryWriter(stream))
					{
						try
						{
							writer.Write(0);
							writer.Write(0);
							writer.Write((short)11);
							writer.Write((byte)6);
							writer.Write(id);
						}
						catch (Exception) { }
						return stream.ToArray();
					}
				}
			}
		}

		public class View : IClientPacket
		{
			public View(int id)
			{
				targetId = id;
			}

			int targetId;
			//0700 0000 2f00 0000 fc00 06
			//Reading packet GAME_REWARDS:13 with id 115 and length 1 data: 1210,1,{"id":1210,"content":{"videos":1},"captions":{"en":"Great shot! Your reward","ru":"Отличный выстрел! Твоя награда"},"max_count":10000,"type":"roulette"}
			//Send packet ROULETTE_ROLL type 252 deviceType 6 id 47 data: 252

			public byte[] ToPack()
			{
				using (var stream = new MemoryStream())
				{
					using (var writer = new BinaryWriter(stream))
					{
						try
						{
							writer.Write(0);
							writer.Write(0);
							writer.Write((short)17);
							writer.Write((byte)6);
							writer.Write(targetId);

							//Console.WriteLine($"view {BitConverter.ToString(stream.ToArray())}");
						}
						catch (Exception) { }
						return stream.ToArray();
					}
				}
			}
		}
	}
}


//public class PacketClient
//{
//       #region STATIC FIELDS
//       // Play
//       public static int PLAY_NEW = 0;
//	public static int PLAY_CHANGE = 1;
//	public static int PLAY_PRIVATE = 2;
//	public static int PLAY_BACK = 3;

//	// Search	   
//	public static int SEARCH_CUSTOM = 0;
//	public static int SEARCH_REGION = 1;
//	// Rating	   
//	public static int RATING_COMMON = 0;
//	public static int RATING_KISSES = 1;
//	public static int RATING_HEARTS = 2;
//	public static int RATING_GIFTS = 3;
//	public static int RATING_ACTIVES = 4;

//	// count 424 reserved by server

//	// Wedding
//	public static int WEDDING_PROPOSAL_YES = 0;
//	public static int WEDDING_PROPOSAL_NO = 1;
//	public static int WEDDING_VOW_YES = 0;
//	public static int WEDDING_VOW_NO = 1;
//	public static int FIANCE_ANSWER_NO = 0;
//	public static int FIANCE_ANSWER_YES = 1;
//	public static int MARRY_GROOM = 0;
//	public static int MARRY_BRIDE = 1;
//	public static int THROW_BOUQUET = 0;
//	public static int THROW_GARTER = 1;
//	public static int CATCH_BOUQUET = 0;
//	public static int CATCH_GARTER = 1;
//	// House	   
//	public static int HOUSE_MOVE_HOLD = 0;
//	public static int HOUSE_MOVE_PLACE = 1;
//	// JOIN_SET	   
//	public static int JOIN_PROFILE = 4;
//	public static int JOIN_ACHIEVEMENT = 8;
//	public static int JOIN_PHOTOS = 11;
//	public static int JOIN_RATING_FORBES = 16;
//	// MAFIA_PLAY  
//	public static int MAFIA_PLAY_NEW = 0;
//	public static int MAFIA_PLAY_PRIVATE = 1;
//	public static int MAFIA_PLAY_CHANGE = 2;
//	// PHOTO_RATE  
//	public static int PHOTO_RATE_DISLIKE = 0;
//	public static int PHOTO_RATE_LIKE = 1;
//	// PETS_ACTION 
//	public static int PET_ACTION_TYPE_NORMAL = 0;
//	public static int PET_ACTION_TYPE_SUPER = 1;
//	// room type for actions
//	public static int ROOM_BOTTLE = 0;
//	public static int ROOM_HOUSE = 1;
//	public static int ROOM_WEDDING = 2;
//	public static int ROOM_MAFIA = 3;
//	public static int ROOM_ANY = 4;
//	// PhotoModeraint tion
//	public static int MODERATION_APPROVE = 0;
//	public static int MODERATION_BLOCK = 1;
//	public static int MODERATION_ERROR = 2;
//	// Kiss		   
//	public static int KISS_ANSWER_NO = 0;
//	public static int KISS_ANSWER_YES = 1;
//	public static int KISS_ANSWER_UNKNOWN = 2;



//	/* tslint:disable:member-ordering */
//	public static string[] FORMATS = new string[] {
//	"",             // MIN_TYPE;

//	// Admin
//	"I",                // ADMIN_REQUEST(1); inner_id:I
//	"IS",               // ADMIN_EDIT(2); player_id:I, json_data:S
//	"S",                // ADMIN_MESSAGE(3); message:S

//	"LBBS,WSIIBSBS",        // LOGIN(4); net_id:L, type:B, device:B, auth_key:S, oauth:W, session_key:S, referrer:I, tag:I, appicationID:B, timestamp:S, language:B, utm_source:S
//	"",             // REFILL(5);
//	"IIII,SS",          // BUY(6); good_id:I, cost:I, target_id:I, data:I, hash:S, params: S
//	"IIIBS,S",          // VIP_GIFT(7); gift_id:I, cost:I, target_id:I, private:B, message:S, hash:S
//	"[I]I,I",           // REQUEST(8); [player_id:I], mask1:I, mask2:I
//	"[L]BI,I",          // REQUEST_NET(9); [net_id:L], type:B, mask1:I, mask2:I
//	"",             // (10); Removed
//	"I",                // GAME_REWARDS_GET(11); reward_id:I
//	"SSSSB",            // INFO(12); name:S, profile:S, status:S, instagram:S, region:B
//	"S",                // PARAMS_SET(13); params:S
//	"I,BI",             // NOTIFY(14); target_id:I, type:B, gift_id:I
//	"L",                // INVITE(15); inviter_id:L
//	"B",                // SEX(16); sex: B
//	"I",                // VIEW(17); inner_id:I
//	"I",                // BIRTHDAY(18); bday:I
//	"B",                // COLOR(19); color:B
//	"W,L",              // COUNT(20); type:W, data:L
//	"IB",               // MOVE(21); target_id:I, destination:B
//	"I",                // TAG(22); value:I
//	"BB",               // LEADERBOARD_REQUEST(23); type:B, period:B
//	"[I]",              // ROOM_INVITE(24); [player_id:I]
//	"BB",               // FLAGS_SET(25); flag_id:B, value:B

//	// Bottle
//	"B,B",              // BOTTLE_PLAY(26); type:B, language:B
//	"",             // BOTTLE_LEAVE(27);
//	"I",                // BOTTLE_ROLL(28); speed:I
//	"B",                // BOTTLE_KISS(29); answer:B
//	"I",                // BOTTLE_SAVE(30); target_id:I
//	"I",                // BOTTLE_KICK(31); player_id:I
//	"B",                // BOMB_HURL(32); room_type:B
//	"",             // BOTTLE_WAITER_JOIN(33);
//	"",             // BOTTLE_WAITER_LEAVE(34);

//	// Chat
//	"",             // CHAT_JOIN(35);
//	"",             // CHAT_LEAVE(36);
//	"BS,II",            // CHAT_MESSAGE(37); chat_type:B, message:S, owner_id:I, data:I
//	"IS",               // CHAT_WHISPER(38); target_id:I, message:S

//	// History
//	"I",                // HISTORY_REQUEST(39); target_id:I
//	"I",                // HISTORY_CLEAR(40); target_id:I

//	"I",                // UNFRIEND(41); friend_id:I
//	"",             // (42); Removed
//	"",             // TOP(43);

//	// Dating
//	"",             // (44); Removed
//	"",             // (45); Removed
//	"",             // (46); Removed
//	"",             // (47); Removed
//	"",             // (48); Removed
//	"",             // (49); Removed
//	"",             // (50); Removed
//	"",             // (51); Removed
//	"",             // (52); Removed
//	"",             // (53); Removed

//	// Anonym
//	"[I]",              // BOTTLE_LEAVING_ROOMS(54); list:[I]
//	"",             // (55); Removed
//	"",             // (56); Removed
//	"",             // (57); Removed

//	// Ignore
//	"I",                // IGNORE_ADD(58); target_id:I
//	"I",                // IGNORE_REMOVE(59); target_id:I

//	"",             // (60); Removed

//	"",             // RECEIVE_DAILY_BONUS(61);

//	// Wedding
//	"IBISI",            // WEDDING_PROPOSAL(62); target_id:I, ring:B, cost:I, message:S, delay:I
//	"IB",               // WEDDING_PROPOSAL_ANSWER(63); target_id:I, answer:B
//	"I",                // WEDDING_PROPOSAL_CANCEL(64); target_id:I
//	"[I]",              // WEDDING_REQUEST(65); [wedding_ids:I]
//	"[I]",              // WEDDING_STATS(66); [wedding_ids:I]
//	"",             // WEDDING_ITEMS_GET(67);
//	"BBI",              // WEDDING_ITEMS_SET(68); type:B, item_id:B, price:I
//	"I",                // WEDDING_INVITE(69); target_id:I
//	"",             // WEDDING_GET_GUESTS(70);
//	"I",                // WEDDING_PLAY(71); wedding_id:I
//	"",             // WEDDING_LEAVE(72);
//	"",             // WEDDING_ROLL(73);
//	"B",                // WEDDING_KISS(74); answer:B
//	"I",                // WEDDING_KICK(75); player_id:I
//	"",             // (76); Removed
//	"",             // WEDDING_WAITER_JOIN(77);
//	"",             // WEDDING_WAITER_LEAVE(78);
//	"",             // WEDDING_START(79);
//	"B",                // WEDDING_VOW_ANSWER(80); answer:B
//	"",             // WEDDING_GARTER_THROW(81);
//	"",             // WEDDING_GARTER_CATCH(82);
//	"",             // WEDDING_BOUQUET_THROW(83);
//	"",             // WEDDING_BOUQUET_CATCH(84);
//	"I",                // WEDDING_CAKE(85); bid:I
//	"I",                // WEDDING_DIVORCE(86); cost:I
//	"I",                // WEDDING_RATING_HAPPY(87); page:I
//	"",             // WEDDING_CANCEL(88);
//	"I",                // WEDDING_REQUEST_RING(89); wedding_id:I

//	"",             // ACHIEVE_POST(90);

//	// House
//	"[I]I",             // HOUSE_REQUEST(91); [room_id:I], mask:I
//	"II",               // HOUSE_MEMBER_ADD(92); houseId:I, target_id:I
//	"II",               // HOUSE_MEMBER_REMOVE(93); houseId:I, target_id:I
//	"",             // (94); Removed
//	"IWI",              // HOUSE_ITEM_ADD(95); room_id:I, type:W, cost:I
//	"",             // (96); Removed
//	"",             // (97); Removed
//	"[IWWWB]",          // HOUSE_ITEM_MOVE(98); [id:I, sector:W, pos_x:W, pos_y:W, rotation:B]
//	"[I]B",             // HOUSE_ITEM_BOXROOM(99); [id:I], state:B
//	"",             // HOUSE_ITEM_BOXROOM_UNSET(100);
//	"",             // HOUSE_ITEM_ACTIVATE(101);
//	"",             // HOUSE_ITEM_DEACTIVATE(102);
//	"I",                // HOUSE_JOIN(103); room_id:I
//	"",             // HOUSE_LEAVE(104);
//	"WWWB",             // HOUSE_MOVE(105); sector:W, pos_x:W, pos_y:W, type:B
//	"",             // HOUSE_WAITER_JOIN(106);
//	"",             // HOUSE_WAITER_LEAVE(107);
//	"",             // (108); Removed
//	"I",                // HOUSE_KNOCK(109); target_id:I
//	"BI",               // HOUSE_MANSIONS(110); is_daily:B, page:I

//	// Curios gifts
//	"",             // CURIOS_REQUEST(111);
//	"III",              // CURIOS_GIFT(112); curio_id:I, cost:I, target_id:I

//	"I",                // ADMIN_CLEAR_ROOM(113); room_id:I

//	"B",                // GET_CHAT_HISTORY(114); chat_type:B
//	"B",                // COLLECTIONS_ASSEMBLE(115); assemble_id:B
//	"",             // (116); Removed
//	"",             // (117); Removed
//	"",             // (118); Removed
//	"",             // (119); Removed
//	"",             // (120); Removed
//	"",             // (121); Removed
//	"",             // (122); Removed
//	"IB",               // HOUSE_ACCESS_PUBLIC(123); houseId:I, available:B
//	"I",                // HOUSE_ACCESS_REQUEST(124); room_id:I
//	"",             // (125); Removed
//	"B",                // TITLE_AWARD(126); award_type: B
//	"",             // (127); Removed
//	"BII",              // TITLE_REQUEST(128); type:B, offset:I, count:B
//	"",             // (129); Removed
//	"",             // (130); Removed
//	"",             // (131); Removed
//	"IBI",              // HOUSE_PACK_ADD(132); room_id:I, type:B, cost:I
//	"S",                // (133); PUSH_TOKEN(133); token:S
//	"",             // (134); Removed
//	"",             // (135); Removed
//	"",             // (136); Removed
//	"I",                // HOUSE_ACTION(137); action_id:I
//	"BB",               // HOUSE_SECTOR(138); sector_id:B, type:B

//	// Moderation
//	"",             // (139); Removed
//	"",             // (140); Removed
//	"",             // (141); Removed
//	"",             // (142); Removed
//	"",             // (143); Removed
//	"",             // SPECTATOR(144);
//	"",             // (145); Removed
//	"B",                // TRAINING_SET(146); type:B
//	"[SS]",             // SING_XS(147); [key:S, data:S]
//	"",             // (148); Removed
//	"",             // (149); Removed
//	"",             // WEDDING_UPDATE(150);
//	"",             // (151); Removed
//	"",             // (152); Removed
//	"",             // (153); Removed
//	"B",                // JOIN_SET(154); set:B
//	"",             // (155); Removed
//	"",             // (156); Removed
//	"",             // (157); Removed
//	"",             // (158); Removed
//	"",             // (159); Removed
//	"",             // (160); Removed
//	"",             // (161); Removed
//	"",             // (162); Removed
//	"",             // (163); Removed
//	"",             // (164); Removed
//	"",             // (165); Removed
//	"",             // (166); Removed
//	"",             // (167); Removed
//	"",             // (168); Removed
//	"",             // (169); Removed
//	"B",                // POST_AWARD(170); type:B
//	"",             // (171); Removed
//	"",             // HALLOWEEN_USE_SUIT (172);
//	"",             // HALLOWEEN_CRAFT_SUIT (173);
//	"BI,BB",            // SET_ACTOIN_TIMEOUT(174); type:B, time:I, event_type:B, text_id:B
//	"",             // (175); Removed
//	"[W]",              // COUNTER_GET(176); [type:W]
//	"",             // (177); Removed
//	"",             // (178); Removed
//	"",             // (179); Removed
//	"",             // (180); Removed
//	"",             // (181); Removed
//	"",             // (182); Removed
//	"BBI",              // RATING(183); is_daily:B, type:B, page:I
//	"",             // (184); Removed
//	"",             // (185); Removed
//	"",             // (186); Removed
//	"",             // (187); Removed
//	"",             // (188); Removed
//	"",             // (189); Removed
//	"",             // (190); Removed
//	"",             // (191); Removed
//	"",             // (192); Removed
//	"",             // (193); Removed
//	"",             // (194); Removed
//	"",             // (195); Removed
//	"",             // (196); Removed
//	"",             // (197); Removed
//	"",             // (198); Removed
//	"",             // (199); Removed
//	"",             // (200); Removed
//	"",             // (201); Removed
//	"I",                // BOTTLE_MOVE(202); room_id: I
//	"I",                // REQUEST_PLAYER_ROOM_TYPE(203); player_id:I

//	// Photos
//	"BS,B",             // PHOTOS_ADD_PHOTO(204); photo_id:B, photo_url:S, active_id:B
//	"B,I",              // PHOTOS_REMOVE_PHOTO(205); photo_id:B, player_id:I
//	"IB",               // PHOTOS_REQUEST(206); player_id:I, photo_id:B
//	"IBB",              // PHOTOS_LIKE(207); player_id:I, photo_id:B, is_like:B
//	"IBS",              // PHOTOS_ADD_COMMENT(208); player_id:I, photo_id:B, text:S
//	"IBI",              // PHOTOS_REMOVE_COMMENT(209); player_id:I, photo_id:B, comment_id:I
//	"IBB",              // PHOTO_RATES_LIKE(210); player_id:I, photo_id:B, is_like:B
//	"",             // PHOTO_RATES_NEXT(211);
//	"I",                // PHOTO_RATES_RATING(212); page:I
//	"IB",               // PHOTO_RATES_FAILED(213); player_id:I, photo_id:B
//	"BB",               // EMOTION(214); emotion:B, room_type:B
//	"",             // PHOTOS_DIFFERENCE(215);
//	"S",                // EMAIL(216); email:S

//	// Admin
//	"BSIB,BBII",            // ADD_NOTIFICATION(217); priority:B, message:S, player_id:I, net_type:B, sex:B, age:B, logout_time_after:I, logout_time_before:I

//	"B",                // SET_PROFILE_BG(218); background_id:B

//	"",             // (219); Removed
//	"",             // (220); Removed
//	"BI",               // WALL_POST(221);
//	"",             // (222); Removed
//	"B",                // SET_FRAME(223); frame_id:B
//	"",             // (224); Removed
//	"I",                // RATING_ELITE(225); page:I
//	"",             // (226); Removed
//	"",             // (227); Removed
//	"",             // (228); Removed
//	"",             // QUEST_AWARD_GET(229);
//	"",             // MODERATION_REQUEST(230);
//	"IBSB",             // MODERATION_DECISION(231); player_id:I, photo_id:B, photo_url:S, decision:B
//	"",             // (232); Removed
//	"B",                // TIMEZONE(233); time_zone:B
//	"BSWS",             // VIDEO_PLAY(234); type:B, video_id:S, duration:W, title:S
//	"",             // (235); Removed
//	"",             // (236); Removed
//	"SB",               // VIDEO_LIKE(237); video_id:S, type:B
//	"II",               // HOLIDAY_ITEM_SEND(238); target_id:I, data:I
//	"III",                  // OFFER_GIFT(239); offer_id:I, target_id:I, gift_id:I
//	"",             // REFILL_SUBSCRIPTION(240);
//	"",             // REQUEST_BOX_INFO(241);
//	"I",                // BIRTHDAY_NOTIFY(242); target:I
//	"",             // (243); Removed
//	"",             // (244); Removed
//	"B",                // OPEN_CHEST(245); chest_id:B
//	"",             // (246); Removed
//	"I[B],S",           // BAN_MULTIPLE(247); target_id:I, [reason:B], link:S
//	"",             // (248); Removed
//	"",             // (249); Removed
//	"",             // (250); Removed
//	"IS",               // ADMIN_BUYINGS_REQUEST(251); player_id:I, date:S
//	"II",               // ADMIN_WELL(252); inner_id:I, count_day:I
//	"",             // HOLIDAY_ITEM_CLICK(253);
//	"I",                // HOLIDAY_RATING_REQUEST(254); page:I
//	"I",                // PHOTOS_ASK(255); player_id:I
//	"I",                // GOAL_REACH(256); target_index:I
//	"BS",               // VIDEO_UP(257); type:B, video_id:S
//	"BIS",              // (258); Removed
//	"I",                // WINK(259); player_id:I
//	"B",                // EMAIL_BONUS(260); type:B
//	"B",                // CAPTCHA(261); answer:B
//	"I",                // ACTION(262); type:I
//	"",             // VIDEO_AD_COMPLETE(263);
//	"BS",               // GIF(264); chat_type:B, gif_id:S
//	"IS",               // GIF_WHISPER(265); target_id:I, gif_id:S
//	"",             // GET_ADMIRE_BONUS(266);
//	"",             // (267); Removed
//	""				// REQUEST_OFFERS(268);
//};
//	#endregion

//	public PacketClient(){}

//	public static async Task<byte[]> CreatePacket(ClientPacketType typePacket, Stack<object> rest)
//	{
//		var type = (short)typePacket;
//		var buffer = new byte[512];
//		var packetLen = 6;

//		using (var stream = new MemoryStream(buffer))
//		{
//			await stream.WriteAsync(BitConverter.GetBytes(0));
//			await stream.WriteAsync(BitConverter.GetBytes(type));

//			string format = PacketClient.FORMATS[type];
//			bool optional = false;
//			int groupPos = 0;
//			int groupLength = 0;

//			Stack<object> container = rest;

//			for (var i = 0; i < format.Length; i++)
//			{
//				char symbol = format[i];

//				if (symbol == ',')
//				{
//					if (optional || groupPos != 0)
//						throw new Exception("Bad signature for client packet " + type);

//					optional = true;
//					continue;
//				}

//				if (symbol == ']')
//				{
//					if (groupPos == 0)
//						throw new Exception("Bad signature for client packet " + type);

//					groupLength--;

//					if (groupLength != 0)
//					{
//						i = groupPos - 1;
//						continue;
//					}

//					groupPos = 0;
//					container = rest;
//					continue;
//				}


//				if (symbol == '[')
//				{
//					if (groupPos != 0)
//						throw new Exception("Bad signature for server packet " + type);

//					int last = getGroupLast(format, i);

//					rest.Pop();
//					container = rest;

//					if (optional && container == null)
//						break;

//					int count = last - i - 1;

//					if (container.Count % count != 0)
//						throw new Exception("Group incomplete for client packet " + type);

//					groupLength = container.Count / count;
//						stream.Write(BitConverter.GetBytes((short)groupLength));

//					if (groupLength != 0)
//					{
//						groupPos = i + 1;
//						continue;
//					}

//					i = last;
//					container = rest;
//					continue;
//				}

//				if (container.Count == 0)
//				{
//					if (optional && groupPos == 0)
//						break;

//					throw new Exception("No data for client packet " + type);
//				}

//				var value = container.Pop();
//				switch (symbol)
//				{
//					case 'S':
//						var _string = value.ToString().StringToBytes();
//						await stream.WriteAsync(_string);
//						stream.WriteByte(0x00);
//						packetLen += _string.Length + 1;
//						break;
//					case 'L':
//						var __ulong = ulong.Parse(value.ToString());
//						var _ulong = BitConverter.GetBytes(__ulong);
//						await stream.WriteAsync(_ulong);
//						packetLen += _ulong.Length;
//						break;
//					case 'I':
//						var _int = BitConverter.GetBytes((int)value);
//						await stream.WriteAsync(_int);
//						packetLen += _int.Length;
//						break;
//					case 'W':
//						var _short = BitConverter.GetBytes((short)value);
//						await stream.WriteAsync(_short);
//						packetLen += _short.Length;
//						break;
//					case 'B':							
//						var _byte = BitConverter.GetBytes(Convert.ToByte(value));
//						stream.WriteByte(_byte[0]);
//						packetLen += 1;
//						break;
//					default:
//						break;
//				}


//			}
//		}

//		var result = buffer.Take(packetLen).ToArray();
//		return result;
//	}

//	private static int getGroupLast(string format,int first)
//	{
//		for (int last = first + 1; last < format.Length; last++)
//		{
//			if (format[last] != ']')
//				continue;

//			if (last == first + 1)
//				break;

//			return last;
//		}

//		throw new Exception("Bad signature for client packet ");
//	}
//}


