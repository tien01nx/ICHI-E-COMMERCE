import { Component } from '@angular/core';
import { ClientFooterComponent } from "../client-footer/client-footer.component";
import { ClientMenuComponent } from "../client-menu/client-menu.component";
import { ClientHeaderComponent } from "../client-header/client-header.component";

@Component({
    selector: 'app-detail-product',
    standalone: true,
    templateUrl: './detail-product.component.html',
    styleUrl: './detail-product.component.css',
    imports: [ClientFooterComponent, ClientMenuComponent, ClientHeaderComponent]
})
export class DetailProductComponent {

}
