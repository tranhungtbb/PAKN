//const BASE_URL = `http://14.177.236.88:6161`;
const BASE_URL = `http://192.168.0.45/PAKNAPI`
export class AppSettings {
	public static HostingAddress = `${BASE_URL}`
	public static weatherApi = 'http://api.openweathermap.org/data/2.5/weather'

	// public static API_ADDRESS = 'http://localhost:51046/api/'
	// public static API_DOWNLOADFILES = 'http://localhost:51046'
	// public static VIEW_FILE = 'http://14.177.236.88:6162/DocViewer?fileurl='
	// public static SIGNALR_ADDRESS = 'http://192.168.0.145/PAKNAPI/signalr'

	public static API_ADDRESS = `${BASE_URL}/api/`
	public static API_DOWNLOADFILES = `${BASE_URL}`
	public static VIEW_FILE = 'http://14.177.236.88:6162/DocViewer?fileurl='
	public static SIGNALR_ADDRESS = `${BASE_URL}/signalr`
}
