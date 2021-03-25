export class PositionObject {
  constructor() {
    this.id = 0;
    this.isDeleted = false;
    this.isActived = true;
    this.description = '';
  }
  id: number;
  name: string;
  code: string;
  isActived: boolean;
  isDeleted: boolean;
  description: string;
  orderNumber: number
}
