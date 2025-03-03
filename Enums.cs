﻿using System;
using System.Collections.Generic;
using System.Text;

namespace bottlelib
{
    public enum NetType : byte
    {
        VK = 0x00,
        MM = 0x01,
        OK = 0x04,
        FB_RU = 0x05,
        MB = 0x06,
        RB = 0x07,
        FB_EN =0x08,
        GM = 0x09,
        VK_PC = 0x0a,
        OK_PC = 0x0b,
        FB_PC = 0x0c,
        FS = 0x1e,
        IM = 0x20,
        AP = 0x28,
        GP = 0x29,
        FB_IG = 0x2a,
        NN = 0xff

    }
    public enum AuthResultType: byte
    {
		 LOGIN_SUCCESS = 0x00,
		 LOGIN_FAILED = 0x01,
		 LOGIN_EXIST = 0x02,
		 LOGIN_BLOCKED = 0x03,
		 LOGIN_WRONG_VERSION = 0x04,
		 UPDATE_GAME = 0x05,
		 ERROR_APP = 0x06,
		 BAN
    }

	public enum DeviceType: byte
	{
		NONE = 0x00,
		DEVICE_PC = 0x01,			// Social PC C# client
		DEVICE_IOS = 0x02,			// AppStore
		DEVICE_ANDROID = 0x03,		// Google Play
		DEVICE_WEB = 0x04,          // https://www.inspin.me
		DEVICE_HTML5 = 0x05,        // Social Mobile
		DEVICE_WEB_MOBILE = 0x06,   // https://m.inspin.me
		DEVICE_PC_NEW = 0x07,       // Social PC TS client
	}
	public enum PlayerTag
    {
        EMPTY = 0,
        FROM_NOTIFICATION = 1,
        HAS_TUTORIAL = 8,
        NO_TUTORIAL = 9,
        CSHARP_CLIENT = 10,
        TS_CLIENT = 11,
        DAILY_BONUS_OLD = 12,
        DAILY_BONUS_NEW = 13,
        GIFTS_NONE_ANIMATION = 14,  // Social only
        GIFTS_HAVE_ANIMATION = 15,  // Social only
        SCREEN_GIFTS_OLD = 16,
        SCREEN_GIFTS_NEW = 17
    }

	public enum Gender: byte
	{
		Undefined = 0x00,
		Woman = 0x01,
		Man = 0x02
	}

	public enum ClientPacketType
	{
		ADMIN_REQUEST = 1,
		ADMIN_EDIT = 2,
		LOGIN = 4,
		REFILL = 5,
		BUY = 6,
		VIP_GIFT = 7,
		REQUEST = 8,
		REQUEST_NET = 9,
		GAME_REWARDS_GET = 11,
		INFO = 12,
		PARAMS = 13,
		NOTIFY = 14,
		INVITE = 15,
		SEX = 16,
		VIEW = 17,
		BIRTHDAY = 18,
		COLOR = 19,
		COUNT = 20,
		MOVE = 21,
		TAG = 22,
		LEADERBOARD_REQUEST = 23,
		ROOM_INVITE = 24,
		FLAGS_SET = 25,
		BOTTLE_PLAY = 26,
		BOTTLE_LEAVE = 27,
		BOTTLE_ROLL = 28,
		BOTTLE_KISS = 29,
		BOTTLE_SAVE = 30,
		BOTTLE_KICK = 31,
		BOMB_HURL = 32,
		BOTTLE_WAITER_JOIN = 33,
		BOTTLE_WAITER_LEAVE = 34,
		CHAT_JOIN = 35,
		CHAT_LEAVE = 36,
		CHAT_MESSAGE = 37,
		CHAT_WHISPER = 38,
		HISTORY_REQUEST = 39,
		HISTORY_CLEAR = 40,
		UNFRIEND = 41,
		TOP = 43,
		BOTTLE_LEAVING_ROOMS = 54,
		IGNORE_ADD = 58,
		IGNORE_REMOVE = 59,
		RECEIVE_DAILY_BONUS = 61,
		WEDDING_PROPOSAL = 62,
		WEDDING_PROPOSAL_ANSWER = 63,
		WEDDING_PROPOSAL_CANCEL = 64,
		WEDDING_REQUEST = 65,
		WEDDING_STATS = 66,
		WEDDING_ITEMS_GET = 67,
		WEDDING_ITEMS_SET = 68,
		WEDDING_INVITE = 69,
		WEDDING_GUESTS = 70,
		WEDDING_PLAY = 71,
		WEDDING_LEAVE = 72,
		WEDDING_ROLL = 73,
		WEDDING_KISS = 74,
		WEDDING_KICK = 75,
		WEDDING_WAITER_JOIN = 77,
		WEDDING_WAITER_LEAVE = 78,
		WEDDING_START = 79,
		WEDDING_VOW_ANSWER = 80,
		WEDDING_GARTER_THROW = 81,
		WEDDING_GARTER_CATCH = 82,
		WEDDING_BOUQUET_THROW = 83,
		WEDDING_BOUQUET_CATCH = 84,
		WEDDING_CAKE = 85,
		WEDDING_DIVORCE = 86,
		WEDDING_RATING_HAPPY = 87,
		WEDDING_CANCEL = 88,
		WEDDING_REQUEST_RING = 89,
		ACHIEVE_POST = 90,
		HOUSE_REQUEST = 91,
		HOUSE_MEMBER_ADD = 92,
		HOUSE_MEMBER_REMOVE = 93,
		HOUSE_ITEM_ADD = 95,
		HOUSE_ITEM_MOVE = 98,
		HOUSE_ITEM_BOXROOM = 99,
		HOUSE_JOIN = 103,
		HOUSE_LEAVE = 104,
		HOUSE_MOVE = 105,
		HOUSE_WAITER_JOIN = 106,
		HOUSE_WAITER_LEAVE = 107,
		HOUSE_KNOCK = 109,
		HOUSE_MANSIONS = 110,
		CURIOS_REQUEST = 111,
		CURIOS_GIFT = 112,
		GET_CHAT_HISTORY = 114,
		COLLECTIONS_ASSEMBLE = 115,
		HOUSE_ACCESS_PUBLIC = 123,
		HOUSE_ACCESS_REQUEST = 124,
		TITLE_AWARD_GET = 126,
		TITLE_REQUEST = 128,
		HOUSE_PACK_ADD = 132,
		PUSH_TOKEN = 133,
		HOUSE_ACTION = 137,
		HOUSE_SECTOR = 138,
		TRAINING_SET = 146,
		SING_XS = 147,
		WEDDING_UPDATE = 150,
		JOIN_SET = 154,
		POST_AWARD = 170,
		HALLOWEEN_USE_SUIT = 172,
		HALLOWEEN_CRAFT_SUIT = 173,
		SET_ACTION_TIMEOUT = 174,
		COUNTER_GET = 176,
		RATING = 183,
		BOTTLE_MOVE = 202,
		REQUEST_PLAYER_ROOM_TYPE = 203,
		PHOTOS_ADD_PHOTO = 204,
		PHOTOS_REMOVE_PHOTO = 205,
		PHOTOS_REQUEST = 206,
		PHOTOS_LIKE = 207,
		PHOTOS_ADD_COMMENT = 208,
		PHOTOS_REMOVE_COMMENT = 209,
		PHOTO_RATES_LIKE = 210,
		PHOTO_RATES_NEXT = 211,
		PHOTO_RATES_RATING = 212,
		PHOTO_RATES_FAILED = 213,
		EMOTION = 214,
		PHOTOS_DIFFERENCE = 215,
		EMAIL = 216,
		SET_PROFILE_BG = 218,
		WALL_POST = 221,
		SET_FRAME = 223,
		RATING_ELITE = 225,
		QUEST_AWARD_GET = 229,
		MODERATION_REQUEST = 230,
		MODERATION_DECISION = 231,
		TIMEZONE = 233,
		VIDEO_PLAY = 234,
		VIDEO_LIKE = 237,
		HOLIDAY_ITEM_SEND = 238,
		OFFER_GIFT = 239,
		REFILL_SUBSCRIPTION = 240,
		BIRTHDAY_NOTIFY = 242,
		OPEN_CHEST = 245,
		BAN_MULTIPLE = 247,
		ADMIN_BUYINGS_REQUEST = 251,
		HOLIDAY_ITEM_CLICK = 253,
		HOLIDAY_RATING_REQUEST = 254,
		PHOTOS_ASK = 255,
		GOAL_REACH = 256,
		VIDEO_UP = 257,
		WINK = 259,
		EMAIL_BONUS = 260,
		CAPTCHA = 261,
		ACTION = 262,
		VIDEO_AD_COMPLETE = 263,
		GIF = 264,
		GIF_WHISPER = 265,
		GET_ADMIRE_BONUS = 266,
		REQUEST_OFFERS = 268
	}
}
