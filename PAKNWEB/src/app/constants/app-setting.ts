//const BASE_URL = `http://14.177.236.88:6161`
const BASE_URL = `http://localhost/PAKNAPI`
export class AppSettings {
	public static HostingAddress = `${BASE_URL}`
	public static weatherApi = 'http://api.openweathermap.org/data/2.5/weather'

	public static API_ADDRESS = `${BASE_URL}/api/`
	public static API_DOWNLOADFILES = `${BASE_URL}`
	public static VIEW_FILE = 'http://14.177.236.88:6162/DocViewer?fileurl='
	public static SIGNALR_ADDRESS = `${BASE_URL}/signalr`
}
