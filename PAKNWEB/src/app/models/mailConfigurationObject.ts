export class MailConfigurationObject {
  constructor() {
    this.id = 0;
    this.name = '';
    this.email = '';
    this.typeMail = 1;
    this.server = '';
    this.port = '';
    this.status = true;
  }

  id: number;
  name: string;
  email: string;
  typeMail: number;
  server: string;
  port: string;
  status: boolean;
}
