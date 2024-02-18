import { Component } from '@angular/core';
import { ClientFooterComponent } from "../client-footer/client-footer.component";
import { ClientMenuComponent } from "../client-menu/client-menu.component";
import { ClientHeaderComponent } from "../client-header/client-header.component";

@Component({
    selector: 'app-products-filter',
    standalone: true,
    templateUrl: './products-filter.component.html',
    styleUrl: './products-filter.component.css',
    imports: [ClientFooterComponent, ClientMenuComponent, ClientHeaderComponent]
})
export class ProductsFilterComponent {

}
