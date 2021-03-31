import { Component, OnInit, ViewChild } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { FieldObject } from 'src/app/models/fieldObject'
import { PositionService } from 'src/app/services/position.service'
import { DataService } from 'src/app/services/sharedata.service'
import { saveAs as importedSaveAs } from 'file-saver'
import { MESSAGE_COMMON, RESPONSE_STATUS } from 'src/app/constants/CONSTANTS'

declare var $: any

@Component({
  selector: 'app-position',
  templateUrl: './position.component.html',
  styleUrls: ['./position.component.css'],
})
export class PositionComponent implements OnInit {
  constructor(private _service: PositionService, private _toastr: ToastrService, private _fb: FormBuilder, private _shareData: DataService) { }

  listData = new Array<FieldObject>()
  listStatus: any = [
    { value: true, text: 'Sử dụng' },
    { value: false, text: 'Không sử dụng' },
  ]
  form: FormGroup
  model: any = new FieldObject()
  submitted: boolean = false
  isActived: boolean
  title: string = ''
  name: string = ''
  description: string = ''
  pageIndex: number = 1
  pageSize: number = 20
  @ViewChild('table', { static: false }) table: any
  totalRecords: number = 0
  idDelete: number = 0
  ngOnInit() {
    this.buildForm()
    this.getList()
  }

  ngAfterViewInit() {
    this._shareData.seteventnotificationDropdown()
  }

  get f() {
    return this.form.controls
  }

  buildForm() {
    this.form = this._fb.group({
      name: [this.model.name, Validators.required],
      description: [this.model.description],
      isActived: [this.model.isActived, Validators.required],
    })
  }

  rebuilForm() {
    this.form.reset({
      name: this.model.name,
      isActived: this.model.isActived,
      description: this.model.description,
    })
  }

  getList() {
    this.name = this.name.trim()

    let request = {
      Name: this.name,
      Description: this.description.trim(),
      isActived: this.isActived != null ? this.isActived : '',
      PageIndex: this.pageIndex,
      PageSize: this.pageSize,
    }
    this._service.positionGetList(request).subscribe((response) => {
      if (response.success == RESPONSE_STATUS.success) {
        if (response.result != null) {
          this.listData = []
          this.listData = response.result.CAPositionGetAllOnPage
          console.log(response)
          this.totalRecords = response.result.CAPositionGetAllOnPage.length != 0 ? response.result.CAPositionGetAllOnPage[0].rowNumber : 0
        }
      } else {
        this._toastr.error(response.message)
      }
    }),
      (error) => {
        console.log(error)
        alert(error)
      }
  }

  onPageChange(event: any) {
    this.pageSize = event.rows
    this.pageIndex = event.first / event.rows + 1
    this.getList()
  }

  dataStateChange() {
    this.pageIndex = 1
    this.table.first = 0
    this.getList()
  }

  changeState(event: any) {
    if (event) {
      if (event.target.value == 'null') {
        this.isActived = null
      } else {
        this.isActived = event.target.value
      }
      this.pageIndex = 1
      this.pageSize = 20
      this.getList()
    }
  }

  changeType(event: any) {
    if (event) {
      this.pageIndex = 1
      this.pageSize = 20
      this.getList()
    }
  }

  preCreate() {
    this.model = new FieldObject()
    this.rebuilForm()
    this.submitted = false
    this.title = 'Thêm mới chức vụ'
    $('#modal').modal('show')
  }

  onSave() {
    this.submitted = true
    if (this.form.invalid) {
      return
    }
    if (this.model.id == 0 || this.model.id == null) {
      this._service.CreatePosition(this.model).subscribe((response) => {
        if (response.success == RESPONSE_STATUS.success) {
          if (response.result == -1) {
            this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
            return
          } else {
            $('#modal').modal('hide')
            this._toastr.success(MESSAGE_COMMON.ADD_SUCCESS)
            this.getList()
          }
        } else {
          this._toastr.error(response.message)
        }
      }),
        (error) => {
          console.error(error)
          alert(error)
        }
    } else {
      this._service.UpdatePosition(this.model).subscribe((response) => {
        if (response.success == RESPONSE_STATUS.success) {
          if (response.result == -1) {
            this._toastr.error(MESSAGE_COMMON.EXISTED_NAME)
            return
          } else {
            $('#modal').modal('hide')
            this._toastr.success(MESSAGE_COMMON.UPDATE_SUCCESS)
            this.getList()
          }
        } else {
          this._toastr.error(response.message)
        }
      }),
        (error) => {
          console.error(error)
          alert(error)
        }
    }
  }

  preUpdate(data) {
    let request = {
      Id: data.id,
      Type: 1,
    }
    this._service.positionGetById(request).subscribe((response) => {
      if (response.success == RESPONSE_STATUS.success) {
        this.rebuilForm()
        this.title = 'Chỉnh sửa chức vụ'
        this.model = response.result.CAPositionGetByID[0]
        $('#modal').modal('show')
      } else {
        this._toastr.error(response.message)
      }
    }),
      (error) => {
        console.error(error)
        alert(error)
      }
  }
  preDelete(id: number) {
    this.idDelete = id
    $('#modalConfirmDelete').modal('show')
  }

  onDelete(id: number) {
    let request = {
      Id: id,
    }
    this._service.positionDelete(request).subscribe((response) => {
      if (response.success == RESPONSE_STATUS.success) {
        this._toastr.success(MESSAGE_COMMON.DELETE_SUCCESS)
        $('#modalConfirmDelete').modal('hide')
        this.getList()
      } else {
        this._toastr.error(response.message)
      }
    }),
      (error) => {
        console.error(error)
      }
  }

  onUpdateStatus(data) {
    var isActived = data.isActived
    let request = {
      Type: 1,
      Id: data.id,
    }
    data.isActived = !data.isActived
    this._service.positionUpdateStatus(data).subscribe((res) => {
      if (res.result == 1) {
        if (data.isActived == true) {
          this._toastr.success(MESSAGE_COMMON.UNLOCK_SUCCESS)
        } else {
          this._toastr.success(MESSAGE_COMMON.LOCK_SUCCESS)
        }
      } else {
        this._toastr.error(res.message)
      }
    }),
      (error) => {
        console.error(error)
      }
  }

  preView(data) {
    this.model = data
    $('#modalDetail').modal('show')
  }
  // exportExcel() {
  //   let request = {
  //     Name: this.name,
  //     IsActived: this.isActived,
  //   }

  //   this._service.fieldExportExcel(request).subscribe((response) => {
  //     var today = new Date()
  //     var dd = String(today.getDate()).padStart(2, '0')
  //     var mm = String(today.getMonth() + 1).padStart(2, '0')
  //     var yyyy = today.getFullYear()
  //     var hh = String(today.getHours()).padStart(2, '0')
  //     var minute = String(today.getMinutes()).padStart(2, '0')
  //     var fileName = 'DM_ChucVuHanhChinh_' + yyyy + mm + dd + hh + minute
  //     var blob = new Blob([response], { type: response.type })
  //     importedSaveAs(blob, fileName)
  //   })
  // }
}
