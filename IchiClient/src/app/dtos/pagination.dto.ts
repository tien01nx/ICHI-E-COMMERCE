export class PaginationDTO<T> {
  content: T[];
  totalPages: number; // Tổng số trang
  totalElements: number; // Tổng số phần tử
  numberOfElements: number; // Số phần tử trên trang hiện tại
  pageSize: number; // Số phần tử trên mỗi trang
  pageNumber: number; // Số trang hiện tại
  firstElementOnPage: number; // Số thứ tự phần tử đầu tiên trên trang hiện tại
  lastElementOnPage: number; // Số thứ tự phần tử cuối cùng trên trang hiện tại
  sortBy: string; // Sắp xếp theo
  sortDirection: string; // Hướng sắp xếp

  constructor(content: T[], totalPages: number, totalElements: number, numberOfElements: number, pageSize: number,
              pageNumber: number, firstElementOnPage: number, lastElementOnPage: number, sortBy: string, sortDirection: string) {
    this.content = content;
    this.totalPages = totalPages;
    this.totalElements = totalElements;
    this.numberOfElements = numberOfElements;
    this.pageSize = pageSize;
    this.pageNumber = pageNumber;
    this.firstElementOnPage = firstElementOnPage;
    this.lastElementOnPage = lastElementOnPage;
    this.sortBy = sortBy;
    this.sortDirection = sortDirection;
  }

  static createEmpty<T>(): PaginationDTO<T> {
    return new PaginationDTO<T>([], 0, 0, 0, 0, 0, 0, 0, "", "");
  }
}
