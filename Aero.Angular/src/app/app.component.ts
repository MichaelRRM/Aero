import { CommonModule } from '@angular/common';
import { Component, ElementRef, HostListener, ViewChild } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  @ViewChild('avatarButtonRef') avatarButtonRef!: ElementRef;
  @ViewChild('settingsMenuRef') settingsMenuRef!: ElementRef;

  isDarkMode = false;

  showSettingsMenu = false;

  toggleSettingsMenu() {
    this.showSettingsMenu = !this.showSettingsMenu;
  }

  toggleDarkMode(event: boolean) {
    this.isDarkMode = event;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    if (!this.showSettingsMenu) return;

    const clickedInsideMenu = this.settingsMenuRef?.nativeElement.contains(event.target);
    const clickedAvatarButton = this.avatarButtonRef?.nativeElement.contains(event.target);

    if (!clickedInsideMenu && !clickedAvatarButton) {
      this.showSettingsMenu = false;
    }
  }
}
