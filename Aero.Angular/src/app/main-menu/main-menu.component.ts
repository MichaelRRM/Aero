import { Component, ElementRef, ViewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-main-menu',
  standalone: true,
  imports: [RouterLink, TranslateModule],
  templateUrl: './main-menu.component.html',
})
export class MainMenuComponent {
  @ViewChild('menuContainer') menuContainer!: ElementRef;
}
