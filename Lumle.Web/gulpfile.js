/// <binding AfterBuild='copy-modules' />
"use strict";

var gulp = require("gulp"),
    clean = require("gulp-clean");
   

var paths = {
    devModules: "../Modules/",
    hostModules: "./Modules/"
};

var modules = [
    "Lumle.Module.CMS",
    "Lumle.Module.Core",
    "Lumle.Module.Error",
    "Lumle.Module.Blog",
    "Lumle.Module.Authorization",
    "Lumle.Module.Audit",
    'Lumle.Module.Localization',
    'Lumle.Module.Calendar',
    'Lumle.Module.AdminConfig',
    'Lumle.Module.PublicUser',
    "Lumle.Module.Schedular"
];

gulp.task("clean-module", function () {
    return gulp.src([paths.hostModules + "*"], { read: false })
    .pipe(clean());
});

gulp.task("copy-modules", ["clean-module"], function () {
    modules.forEach(function (module) {
        gulp.src([paths.devModules + module + "/Views/**/*.*", paths.devModules + module + "/wwwroot/**/*.*"], { base: module })
            .pipe(gulp.dest(paths.hostModules + module));
        gulp.src(paths.devModules + module + "/bin/Debug/netstandard1.6/**/*.*")
            .pipe(gulp.dest(paths.hostModules + module + "/bin"));
    });
});

gulp.task("copy-ms", function () {
    modules.forEach(function (module) {
        gulp.src([paths.devModules + module + "/Views/**/*.*", paths.devModules + module + "/wwwroot/**/*.*"], { base: module })
            .pipe(gulp.dest(paths.hostModules + module));
    });
});
