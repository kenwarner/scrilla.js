var scrilla = scrilla || {};
scrilla.directives = angular.module('scrilla.directives', []);
scrilla.controllers = angular.module('scrilla.controllers', []);
scrilla.services = angular.module('scrilla.services', ['ngResource']);
scrilla.app = angular.module('scrillaApp', ['scrilla.directives', 'scrilla.controllers', 'scrilla.services']);