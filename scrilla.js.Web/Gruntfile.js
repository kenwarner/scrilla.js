module.exports = function (grunt) {

	grunt.initConfig({
		pkg: grunt.file.readJSON('package.json'),

		watch: {
			bower: {
				files: ['Gruntfile.js'],
				tasks: ['bower:install'],
				options: {
					spawn: false
				}
			}
		},

		bower: {
			install: {
				options: {
					targetDir: './lib',
					layout: 'byComponent',
					install: true,
					verbose: false,
					cleanTargetDir: true,
					cleanBowerDir: false,
					bowerOptions: {}
				}
			}
		},

		devUpdate: {
			npm: {
				options: {
					updateType: 'report',
					packages: {
						devDependencies: true,
						dependencies: true
					}
				}
			}
		},

		karma: {
			unit: {
				configFile: 'app/tests/karma.unit.conf.js',
				singleRun: true
			}
		},

		xunit_runner: {
			options: {
				xUnit: '.\\node_modules\\grunt-xunit\\vendor\\xunit\\xunit.console.exe', // in the grunt-xunit package, not grunt-xunit-runner
				silent: true,
				teamcity: true
			},
			files: {
				src: [
					'../*.Tests/bin/**/*.Tests.dll'
				]
			}
		}

	});

	grunt.loadNpmTasks('grunt-bower-task');
	grunt.loadNpmTasks('grunt-contrib-watch');
	grunt.loadNpmTasks('grunt-dev-update');
	grunt.loadNpmTasks('grunt-karma');
	grunt.loadNpmTasks('grunt-xunit-runner');

	grunt.registerTask('default', ['bower', 'devUpdate', 'karma:unit']);
	grunt.registerTask('tests', ['karma:unit']);
};
