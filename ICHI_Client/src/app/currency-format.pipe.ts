import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'currencyFormat',
  standalone: true,
})
export class CurrencyFormatPipe implements PipeTransform {
  transform(value: number): string {
    // Đổi đơn vị tiền tệ từ $ sang đ
    return 'đ' + value.toFixed(2); // Giả sử giá trị đã truyền vào là số nguyên
  }
}
