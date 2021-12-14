export const QBconfig = {
	credentials: {
		appId: '93612',
		authKey: 'nTgwfxrtOwVaF86',
		authSecret: 'YVcQxAKNeYUW8ZE',
		accountKey: 'AyepN2ovQLKPVX1rZ4eq',
	},
	appConfig: {
		chatProtocol: {
			active: 2,
		},
		streamManagement: {
			enable: true,
		},
		debug: {
			mode: 1,
			file: null,
		},
	},
}

export const CONSTANTS = {
	DIALOG_TYPES: {
		CHAT: 3,
		GROUPCHAT: 2,
		PUBLICCHAT: 1,
	},
	ATTACHMENT: {
		TYPE: 'image,application',
		BODY: '[attachment]',
		MAXSIZE: 10 * 1000000, // set 10 megabytes,
		MAXSIZEMESSAGE: 'Image is too large. Max size is 2 mb.',
	},
	NOTIFICATION_TYPES: {
		NEW_DIALOG: '1',
		UPDATE_DIALOG: '2',
		LEAVE_DIALOG: '3',
	},
}
