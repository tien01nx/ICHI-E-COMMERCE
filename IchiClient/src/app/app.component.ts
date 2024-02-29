import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { ClientLayoutComponent } from './components/client/client-layout/client-layout.component';
import { ClientHeaderComponent } from './components/client/client-header/client-header.component';
import { ClientMenuComponent } from './components/client/client-menu/client-menu.component';
import { NgxDropzoneModule } from 'ngx-dropzone';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  imports: [
    CommonModule,
    RouterOutlet,
    ClientLayoutComponent,
    ClientHeaderComponent,
    ClientMenuComponent,
    NgxDropzoneModule,
  ],
})
export class AppComponent {
  title = 'IchiClient';
}
