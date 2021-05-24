import { Injectable } from '@angular/core'
import { TreeviewItem, TreeviewSelection, TreeviewI18n } from 'ngx-treeview'

@Injectable()
export class DefaultTreeviewI18n extends TreeviewI18n {
	constructor() {
		super()
	}

	getText(selection: TreeviewSelection): string {
		if (selection.uncheckedItems.length === 0) {
			if (selection.checkedItems.length > 0) {
				return 'Tất cả'
			} else {
				return 'Chọn thành phần tham dự'
			}
		}

		switch (selection.checkedItems.length) {
			case 0:
				return 'Chọn thành phần tham dự'
			case 1:
				return selection.checkedItems[0].text
			default:
				return `${selection.checkedItems.length} thành phần đã chọn`
		}
	}

	getAllCheckboxText(): string {
		return 'Tất cả'
	}

	getFilterPlaceholder(): string {
		return 'Tìm kiếm'
	}

	getFilterNoItemsFoundText(): string {
		return 'Không có mục nào được tìm thấy'
	}

	getTooltipCollapseExpandText(isCollapse: boolean): string {
		return isCollapse ? 'Mở rộng' : 'Thu lại'
	}
}
