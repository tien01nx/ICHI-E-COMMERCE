import { PaginationDTO } from './../../../dtos/pagination.dto';
import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router, Data } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { Utils } from '../../../Utils.ts/utils';
import { CategoryProduct } from '../../../models/category.product';
import { CategoryService } from '../../../service/category-product.service';
import { TreeNode } from '../../../models/tree.mode';

@Component({
  selector: 'app-supplier.admin',
  templateUrl: './category.component.html',
  styleUrl: './category.component.css',
})
export class CategoryComponent implements OnInit {
  protected readonly Utils = Utils;
  paginationModel: PaginationDTO<CategoryProduct> = PaginationDTO.createEmpty();
  searchTemp: any = this.activatedRoute.snapshot.queryParams['Search'] || '';
  selectAll: boolean = false;
  sortDir: string = 'ASC';
  SortBy: string = 'id';

  categories: any;

  parentIds: CategoryProduct[] = [];

  categoriesLevel: CategoryProduct[] = [];

  tree: TreeNode[] = [];

  @ViewChild('btnCloseModal') btnCloseModal!: ElementRef;
  titleModal: string = '';
  btnSave: string = '';
  isDisplayNone: boolean = false;
  errorMessage: string = '';
  categoryForm: FormGroup = new FormGroup({
    id: new FormControl('0'),
    parentID: new FormControl('0'),
    categoryLevel: new FormControl('0'),
    categoryName: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
      Validators.pattern(Utils.textPattern),
    ]),
  });

  buildTree(
    categories: CategoryProduct[],
    parentID: number = 0,
    level: number = 1
  ): TreeNode[] {
    const nodes: TreeNode[] = [];

    categories.forEach((data: CategoryProduct) => {
      if (data.parentID === parentID) {
        const newNode: TreeNode = {
          key: data.id.toString(),
          label: data.categoryName,
          data: data,
          icon: 'pi pi-fw pi-folder', // Assuming default icon for folder
        };
        const children = this.buildTree(categories, data.id, level + 1);
        if (children.length) {
          newNode.children = children;
        }
        nodes.push(newNode);
      }
    });

    return nodes;
  }
  constructor(
    private title: Title,
    private categoryService: CategoryService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}
  ngOnInit() {
    this.title.setTitle('Quản lý danh mục sản phẩm');
    this.activatedRoute.queryParams.subscribe((params) => {
      const search = params['search'] || '';
      const pageSize = +params['page-size'] || 10;
      const pageNumber = +params['page-number'] || 1;
      const sortDir = params['sort-direction'] || 'ASC';
      const sortBy = params['sort-by'] || '';
      this.findAll(pageSize, pageNumber, sortBy, sortDir, search);
    });
    this.categoryService.findAll().subscribe((data: any) => {
      this.parentIds = data.data;
      console.log(data.data);
    });

    this.categoryService.findAll().subscribe((categories: any) => {
      console.log('categories', categories.data);
      this.tree = this.buildTree(categories.data);
      console.log('tree', this.tree);
    });
  }

  toggleSelectAll() {
    this.selectAll = !this.selectAll;
  }

  findAll(
    pageSize: number,
    pageNumber: number,
    sortBy: string,
    sortDir: string,
    search: string
  ) {
    this.categoryService
      .findAllByName(pageNumber, pageSize, sortDir, sortBy, search)
      .subscribe({
        next: (response: any) => {
          console.log(response);
          //this.paginationModel.content = response.data.items;
          this.categories = response.data.items;
          this.categories.forEach((item: any) => {
            if (!item.parentName) {
              item.parentName = this.categories.find(
                (x: any) => x.id === item.parentID
              )?.categoryName;
            }
          });
          console.log('demo', this.categories);

          this.paginationModel.content = this.categories;

          // this.paginationModel.content.forEach((item) => { !item.parentName : this.parentIds.find((x) => x.id === item.parentID)?.categoryName;});

          this.paginationModel.totalPages = response.data.pageCount;
          this.paginationModel.totalElements = response.data.totalCount;
          this.paginationModel.numberOfElements = response.numberOfElements;
          this.paginationModel.pageSize = response.data.pageSize;
          this.paginationModel.pageNumber = response.data.currentPage;
          this.paginationModel.firstElementOnPage = response.firstElementOnPage;
          this.paginationModel.lastElementOnPage = response.lastElementOnPage;
          this.paginationModel.sortBy = response.sortBy;
          this.paginationModel.sortDirection = response.sortDirection;
          console.log(this.paginationModel);
        },
        error: (error: any) => {
          console.log(error);
        },
      });
  }

  changePageNumber(pageNumber: number): void {
    this.router
      .navigate(['/admin/categoryproduct'], {
        queryParams: { 'page-number': pageNumber },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  changePageSize(pageSize: number): void {
    this.router
      .navigate(['/admin/categoryproduct'], {
        queryParams: { 'page-size': pageSize, 'page-number': 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  sortByField(sortBy: string): void {
    this.router
      .navigate(['/admin/categoryproduct'], {
        queryParams: { 'sort-by': sortBy, 'sort-direction': this.sortDir },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
    this.sortDir = this.sortDir === 'ASC' ? 'DESC' : 'ASC';
    this.SortBy = sortBy;
  }

  search() {
    this.router
      .navigate(['/admin/categoryproduct'], {
        queryParams: { search: this.searchTemp, 'page-number': 1 },
        queryParamsHandling: 'merge',
      })
      .then((r) => {});
  }

  onSubmit() {
    debugger;
    if (this.categoryForm.invalid) {
      return;
    }
    if (this.categoryForm.value.id == null) this.create();
    else this.update();
  }

  create() {
    this.isDisplayNone = true;
    this.categoryForm.value.id = 0;
    this.categoryService.create(this.categoryForm.value).subscribe({
      next: (response: any) => {
        if (response.message === 'Tạo mới danh mục thành công') {
          this.categoryForm.reset();
          this.btnCloseModal.nativeElement.click();
          this.updateTable();
          this.toastr.success(response.message, 'Thông báo');
        } else {
          this.errorMessage = response.message;
          this.isDisplayNone = false;
          this.toastr.error(response.message, 'Thông báo');
        }
      },
      error: (error: any) => {
        this.errorMessage = error.error;
        this.isDisplayNone = false;
      },
    });
  }

  update() {
    this.isDisplayNone = true;
    this.categoryService.update(this.categoryForm.value).subscribe({
      next: (response: any) => {
        console.log(response);
        debugger;
        if (response.message === 'Cập nhật danh mục thành công') {
          this.categoryForm.reset();
          this.btnCloseModal.nativeElement.click();
          this.updateTable();
          this.toastr.success(response.message, 'Thông báo');
        } else {
          this.toastr.error(response.message, 'Thông báo');
        }
      },
      error: (error: any) => {
        this.errorMessage = error.error;
        this.isDisplayNone = false;
      },
    });
  }

  delete(id: number) {
    Swal.fire({
      title: 'Bạn có chắc chắn muốn xóa?',
      text: 'Dữ liệu sẽ không thể phục hồi sau khi xóa!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Xác nhận',
      cancelButtonText: 'Hủy',
      buttonsStyling: false,
      customClass: {
        confirmButton: 'btn btn-danger me-1',
        cancelButton: 'btn btn-secondary',
      },
    }).then((result) => {
      if (result.isConfirmed) {
        this.categoryService.delete(id).subscribe({
          next: (response: any) => {
            this.updateTable();
            this.toastr.success(response.message, 'Thông báo');
          },
          error: (error: any) => {
            this.toastr.error(error.error, 'Thất bại');
          },
        });
      }
    });
  }

  openModalCreate() {
    this.categoryForm.reset();
    this.titleModal = 'Thêm danh mục sàn phẩm';
    this.btnSave = 'Thêm mới';
    this.errorMessage = '';
  }

  openModalUpdate(category: CategoryProduct) {
    this.categoryForm.patchValue({
      id: category.id,
      parentID: category.parentID,
      categoryLevel: category.categoryLevel,
      categoryName: category.categoryName,
    });
    this.titleModal = 'Cập nhật ndanh mục sản phẩm';
    this.btnSave = 'Cập nhật';
  }

  updateTable() {
    this.isDisplayNone = false;
    this.errorMessage = '';
    this.findAll(this.paginationModel.pageSize, 1, '', '', '');
  }

  getCategories(
    categories: CategoryProduct[],
    parentID: number
  ): CategoryProduct[] {
    const result: CategoryProduct[] = [];
    if (parentID >= 0) {
      const parent = categories.find((category) => category.id === parentID);
      if (parent) {
        result.push(parent);
        const children = categories.filter(
          (category) => !category.isDeleted && category.parentID === parentID
        );
        if (children.length > 0) {
          children.forEach((child) => {
            const childCategories = this.getCategories(categories, child.id);
            result.push(...childCategories);
          });
        }
      }
    }
    return result;
  }

  getDistrictsControl(): FormControl {
    const cityControl = this.categoryForm.get('parentID') as FormControl;

    cityControl.valueChanges.pipe().subscribe((id: any) => {
      // this.categoriesLevel?.forEach((data: any) => {
      //   if (data.categoryLevel === id) {
      //     this.categoriesLevel = data.categoryLevel;
      //     // this.categoryForm.get('district')?.setValue(this.districts[0]?.name); // Đảm bảo mảng districts không rỗng trước khi gán giá trị
      //   }
      // });
      this.categoriesLevel = this.getCategories(this.parentIds, id);
      this.categoryForm
        .get('categoryLevel')
        ?.setValue(this.categoriesLevel[0]?.id); // Đảm bảo mảng districts không rỗng trước khi gán giá trị
    });
    return cityControl;
  }
}
