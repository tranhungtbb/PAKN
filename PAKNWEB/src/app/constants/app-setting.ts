export const BASEURL = 'http://192.168.0.45/PAKNAPI/'
export class AppSettings {
	public static HostingAddress = 'http://14.177.236.88:6161'
	public static weatherApi = 'http://api.openweathermap.org/data/2.5/weather'

	// public static API_ADDRESS = 'http://localhost:51046/api/'
	// public static API_DOWNLOADFILES = 'http://localhost:51046'
	// public static VIEW_FILE = 'http://14.177.236.88:6162/DocViewer?fileurl='
	// public static API_ADDRESS = 'http://14.177.236.88:6161/api/'
	// public static API_DOWNLOADFILES = 'http://14.177.236.88:6161'
	// public static VIEW_FILE = 'http://14.177.236.88:6162/DocViewer?fileurl='
	//public static API_ADDRESS = 'http://14.177.236.88:6161/api/'
	public static API_ADDRESS = `${BASEURL}api/`
	public static SIGNALR_ADDRESS = `${BASEURL}signalr/`
	public static API_DOWNLOADFILES = 'http://14.177.236.88:6161'
	public static VIEW_FILE = 'http://14.177.236.88:6162/DocViewer?fileurl='
}
