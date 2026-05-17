import { Routes } from '@angular/router';
import { Nav } from '../layout/nav/nav';
import { Home } from '../features/home/home';
import { TestErrors } from '../features/test-errors/test-errors';
import { NotFound } from '../shared/errors/not-found/not-found';
import { ServerError } from '../shared/errors/server-error/server-error';

export const routes: Routes = [
    {path: '', component: Nav},
    {path: 'dashboard', component: Home},
    {path: 'logout', component: Nav},
    {path: 'errors', component: TestErrors},
    {path: 'not-found', component: NotFound},
    {path: 'server-error', component: ServerError},
];
