import { Component } from '@angular/core';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
@Component({
  selector: 'app-invoice',
  templateUrl: './invoice.component.html',
  styleUrl: './invoice.component.css',
})
export class InvoiceComponent {
  public exportAsPDF() {
    const data = document.getElementById('invoice'); // ID của phần tử bạn muốn xuất
    if (data) {
      html2canvas(data).then((canvas) => {
        const contentDataURL = canvas.toDataURL('image/png');
        let pdf = new jsPDF('p', 'mm', 'a4'); // tạo mới một đối tượng PDF
        let pageWidth = pdf.internal.pageSize.getWidth();
        let pageHeight = pdf.internal.pageSize.getHeight();
        let imageWidth = canvas.width;
        let imageHeight = canvas.height;

        let ratio =
          imageWidth / imageHeight >= 1
            ? pageWidth / imageWidth
            : pageHeight / imageHeight;
        let newWidth = imageWidth * ratio;
        let newHeight = imageHeight * ratio;
        let marginX = (pageWidth - newWidth) / 2;
        let marginY = (pageHeight - newHeight) / 2;

        pdf.addImage(
          contentDataURL,
          'PNG',
          marginX,
          marginY,
          newWidth,
          newHeight
        );
        pdf.save('invoice.pdf');
      });
    }
  }
}
