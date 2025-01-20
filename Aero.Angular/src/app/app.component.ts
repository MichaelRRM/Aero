import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  HostListener,
  OnInit,
  ViewChild,
} from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { RouterOutlet } from '@angular/router';
import { SettingsService } from './services/settings.service';
import { SettingsMenuComponent } from './settings-menu/settings-menu.component';
import { MainMenuComponent } from './main-menu/main-menu.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, TranslateModule, SettingsMenuComponent, MainMenuComponent],
  templateUrl: './app.component.html',
})
export class AppComponent {
  @ViewChild('avatarButtonRef') avatarButtonRef!: ElementRef;
  @ViewChild(SettingsMenuComponent) settingsMenuRef!: SettingsMenuComponent;
  @ViewChild('menuButtonRef') menuButtonRef!: ElementRef;
  showSettingsMenu = false;
  showMainMenu = false;

  constructor(
    public settings: SettingsService,
    private translate: TranslateService
  ) {
    this.translate.addLangs(['fr', 'en']);
    this.translate.setDefaultLang('en');
    this.translate.use('en');
  }

  onDarkModeChange(darkMode: boolean) {
    this.settings.updateSettings({ darkMode });
  }

  onLanguageChange(language: string) {
    this.settings.updateSettings({ language });
  }

  toggleMainMenu() {
    this.showMainMenu = !this.showMainMenu;
    if (this.showSettingsMenu) this.showSettingsMenu = false;
  }

  toggleSettingsMenu() {
    this.showSettingsMenu = !this.showSettingsMenu;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const target = event.target as HTMLElement;

    if (this.showMainMenu) {
      const clickedHamburger = this.menuButtonRef?.nativeElement.contains(target);
      const clickedInsideMenu = target.closest('.main-menu');

      if (!clickedHamburger && !clickedInsideMenu) {
        this.showMainMenu = false;
      }
    }

    if (!this.showSettingsMenu) return;
    const clickedInsideMenu = this.settingsMenuRef?.menuContainer.nativeElement.contains(target);
    const clickedAvatarButton = this.avatarButtonRef?.nativeElement.contains(target);
    if (!clickedInsideMenu && !clickedAvatarButton) {
      this.showSettingsMenu = false;
    }
  }
}
