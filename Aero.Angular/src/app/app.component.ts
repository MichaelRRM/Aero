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

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, TranslateModule, SettingsMenuComponent],
  templateUrl: './app.component.html',
})
export class AppComponent {
  @ViewChild('avatarButtonRef') avatarButtonRef!: ElementRef;
  @ViewChild(SettingsMenuComponent) settingsMenuRef!: SettingsMenuComponent;
  showSettingsMenu = false;

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

  toggleSettingsMenu() {
    this.showSettingsMenu = !this.showSettingsMenu;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    if (!this.showSettingsMenu) return;
    const clickedInsideMenu = this.settingsMenuRef?.menuContainer.nativeElement.contains(event.target);
    const clickedAvatarButton = this.avatarButtonRef?.nativeElement.contains(event.target);
    if (!clickedInsideMenu && !clickedAvatarButton) {
      this.showSettingsMenu = false;
    }
  }
}
