import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
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

  private settings = new BehaviorSubject<UserSettings>(this.initialSettings);
  settings$ = this.settings.asObservable();

  constructor(private translate: TranslateService) {
    this.translate.use(this.initialSettings.language);
  }

  updateSettings(newSettings: Partial<UserSettings>) {
    const currentSettings = this.settings.getValue();
    const updatedSettings = { ...currentSettings, ...newSettings };
    this.settings.next(updatedSettings);

    if (newSettings.darkMode !== undefined) {
      localStorage.setItem('darkMode', newSettings.darkMode.toString());
    }
    if (newSettings.language !== undefined) {
      localStorage.setItem('language', newSettings.language);
      this.translate.use(newSettings.language);
    }
  }
}
