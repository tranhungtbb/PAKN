//const BASE_URL = `http://14.177.236.88:6161`
const BASE_URL = `https://pakn.ziz.vn:6164`
//const BASE_URL = `http://localhost/PAKNAPI`
// const BASE_URL = `http://localhost:51046/`

export class AppSettings {
	public static HostingAddress = `${BASE_URL}`
	public static weatherApi = 'http://api.openweathermap.org/data/2.5/weather'

	public static API_ADDRESS = `${BASE_URL}/api/`
	public static API_DOWNLOADFILES = `${BASE_URL}`
	public static VIEW_FILE = 'https://pakn.ziz.vn:6162/DocViewer?fileurl='
	public static SIGNALR_ADDRESS = `${BASE_URL}/signalr`
}
