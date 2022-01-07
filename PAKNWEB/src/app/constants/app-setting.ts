//const BASE_URL = `http://14.177.236.88:6161`
//const BASE_URL = `https://pakn.ziz.vn:6164`
const BASE_URL = `https://tuongtac.khanhhoa.gov.vn:6899`
//const BASE_URL = `http://localhost/PAKNAPI`

export class AppSettings {
	public static HostingAddress = `${BASE_URL}`
	public static weatherApi = 'http://api.openweathermap.org/data/2.5/weather'

	public static API_ADDRESS = `${BASE_URL}/api/`
	public static API_DOWNLOADFILES = `${BASE_URL}`
	public static VIEW_FILE = 'https://pakn.ziz.vn:6162/DocViewer?fileurl='
	//public static VIEW_FILE = 'https://tuongtac.khanhhoa.gov.vn:8699/DocViewer?fileurl='
	public static SIGNALR_ADDRESS = `${BASE_URL}/signalr`
}
