import { Component } from '@angular/core';
import { EditorModule } from '@tinymce/tinymce-angular';

@Component({
  selector: 'app-insert-admin-product',
  standalone: true,
  imports: [EditorModule],
  templateUrl: './insert-admin-product.component.html',
  styleUrl: './insert-admin-product.component.css',
})
export class InsertAdminProductComponent {}
