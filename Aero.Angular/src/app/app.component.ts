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
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, TranslateModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  @ViewChild('avatarButtonRef') avatarButtonRef!: ElementRef;
  @ViewChild('settingsMenuRef') settingsMenuRef!: ElementRef;

  isDarkMode = false;
  selectedLanguage = 'en';
  showSettingsMenu = false;

  constructor(private translate: TranslateService) {
    this.translate.addLangs(['fr', 'en']);
    this.translate.setDefaultLang('en');
    this.translate.use('en');
  }

  ngOnInit(): void {
    this.isDarkMode = window.matchMedia('(prefers-color-scheme: dark)').matches;

    const savedDarkMode = localStorage.getItem('darkMode');
    if (savedDarkMode) {
      this.isDarkMode = savedDarkMode === 'true';
    }

    const savedLang = localStorage.getItem('language');
    if (savedLang) {
      this.selectedLanguage = savedLang;
      this.translate.use(savedLang);
    }
  }

  toggleSettingsMenu() {
    this.showSettingsMenu = !this.showSettingsMenu;
  }

  toggleDarkMode(event: boolean) {
    this.isDarkMode = event;
    localStorage.setItem('darkMode', event.toString());
  }

  onLanguageChange(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    this.selectedLanguage = selectElement.value;
    this.translate.use(this.selectedLanguage);
    localStorage.setItem('language', this.selectedLanguage);
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    if (!this.showSettingsMenu) return;

    const clickedInsideMenu = this.settingsMenuRef?.nativeElement.contains(
      event.target
    );
    const clickedAvatarButton = this.avatarButtonRef?.nativeElement.contains(
      event.target
    );

    if (!clickedInsideMenu && !clickedAvatarButton) {
      this.showSettingsMenu = false;
    }
  }
}
