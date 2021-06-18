export const QBconfig = {
	credentials: {
		appId: '91420',
		authKey: 'xF9GaN2w8z6ahVG',
		authSecret: 'xbGpYtfGLbDmypw',
		accountKey: 'LSFZza7dNxHVo89yx415',
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
