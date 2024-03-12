import { Component } from '@angular/core';
import { ClientFooterComponent } from "../client-footer/client-footer.component";
import { ClientMenuComponent } from "../client-menu/client-menu.component";
import { ClientHeaderComponent } from "../client-header/client-header.component";

@Component({
    selector: 'app-checkout',
    standalone: true,
    templateUrl: './checkout.component.html',
    styleUrl: './checkout.component.css',
    imports: [ClientFooterComponent, ClientMenuComponent, ClientHeaderComponent]
})
export class CheckoutComponent {

}
