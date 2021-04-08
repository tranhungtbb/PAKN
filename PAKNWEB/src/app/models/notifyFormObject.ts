export class NotifyFormObject {
  constructor() {
    this.id = 0;
    this.name = '';
    this.title = '';
    this.content = null;
    this.action = 1;
    this.status = true;
  }

  id: number;
  name: string;
  title: string;
  content: number;
  action: number;
  status: boolean;
}
