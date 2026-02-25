import { Routes } from '@angular/router';
import { Nav } from '../layout/nav/nav';
import { Home } from '../features/home/home';

export const routes: Routes = [
    {path: '', component: Nav},
    {path: 'dashboard', component: Home},
    {path: 'logout', component: Nav}
];
