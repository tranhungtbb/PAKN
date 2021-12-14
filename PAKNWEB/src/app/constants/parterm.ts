export class Parterms {
	public static specialCharacter = /[!@#$%^&*()+\-=\[\]{};':"\\|,<>\/?]+/ /*/[!@#$%^&*()+\-=\[\]{};':"\\|,<>\/?]+/;*/

	public static specialCharacter1 = /[!@#$%^&*()+\=\[\]{};':"\\|,<>\/?]+/

	public static specialCharacter2 = /[!@#$%^&*()+\=\[\]{};':"\\|,<>\/?]+/

	public static specialCharacter3 = /[!@#$%^&*()+\=\[\]{};':"\\|,<>\?]+/

	public static email = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
}
