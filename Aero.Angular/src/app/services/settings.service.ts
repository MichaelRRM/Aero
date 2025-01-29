import { Injectable, signal, computed } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

export interface UserSettings {
  darkMode: boolean;
  language: string;
}

@Injectable({
  providedIn: 'root'
})
export class SettingsService {
  private initialSettings: UserSettings = {
    darkMode: localStorage.getItem('darkMode') === 'true',
    language: localStorage.getItem('language') || 'en'
  };

  private settings = signal<UserSettings>(this.initialSettings);

  readonly darkMode = computed(() => this.settings().darkMode);
  readonly language = computed(() => this.settings().language);

  constructor(private translate: TranslateService) {
    this.translate.use(this.initialSettings.language);
  }

  updateSettings(newSettings: Partial<UserSettings>) {
    this.settings.update(currentSettings => {
      const updatedSettings = { ...currentSettings, ...newSettings };

      if (newSettings.darkMode !== undefined) {
        localStorage.setItem('darkMode', newSettings.darkMode.toString());
      }
      if (newSettings.language !== undefined) {
        localStorage.setItem('language', newSettings.language);
        this.translate.use(newSettings.language);
      }

      return updatedSettings;
    });
  }
}
