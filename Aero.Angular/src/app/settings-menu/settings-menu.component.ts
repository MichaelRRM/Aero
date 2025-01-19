import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { SettingsService } from '../services/settings.service';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-settings-menu',
  standalone: true,
  imports: [CommonModule, TranslateModule, FormsModule],
  templateUrl: './settings-menu.html',
})
export class SettingsMenuComponent {
  @ViewChild('settingsMenuRef') menuContainer!: ElementRef;

  constructor(
    public settings: SettingsService,
    public translate: TranslateService
  ) {}

  onDarkModeChange(darkMode: boolean) {
    this.settings.updateSettings({ darkMode });
  }

  onLanguageChange(language: string) {
    this.settings.updateSettings({ language });
  }
}
