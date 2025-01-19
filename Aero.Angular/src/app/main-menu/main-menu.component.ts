import { Component, ElementRef, ViewChild } from '@angular/core';

@Component({
  selector: 'app-main-menu',
  standalone: true,
  imports: [],
  templateUrl: './main-menu.component.html',
})
export class MainMenuComponent {
  @ViewChild('menuContainer') menuContainer!: ElementRef;
}
