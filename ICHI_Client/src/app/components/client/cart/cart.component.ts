import { Component } from '@angular/core';
import { ClientFooterComponent } from "../client-footer/client-footer.component";
import { ClientHeaderComponent } from "../client-header/client-header.component";
import { ClientMenuComponent } from "../client-menu/client-menu.component";

@Component({
    selector: 'app-cart',
    standalone: true,
    templateUrl: './cart.component.html',
    styleUrl: './cart.component.css',
    imports: [ClientFooterComponent, ClientHeaderComponent, ClientMenuComponent]
})
export class CartComponent {

}
