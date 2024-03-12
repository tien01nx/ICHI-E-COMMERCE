import { Component } from '@angular/core';
import { ClientHeaderComponent } from "../client-header/client-header.component";
import { ClientMenuComponent } from "../client-menu/client-menu.component";
import { ClientFooterComponent } from "../client-footer/client-footer.component";

@Component({
    selector: 'app-shipping-info',
    standalone: true,
    templateUrl: './shipping-info.component.html',
    styleUrl: './shipping-info.component.css',
    imports: [ClientHeaderComponent, ClientMenuComponent, ClientFooterComponent]
})
export class ShippingInfoComponent {

}
