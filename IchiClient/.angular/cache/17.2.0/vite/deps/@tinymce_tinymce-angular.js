import {
  FormsModule,
  NG_VALUE_ACCESSOR
} from "./chunk-AEIEYWUM.js";
import {
  CommonModule,
  isPlatformBrowser
} from "./chunk-NHJGMXCN.js";
import {
  Component,
  Directive,
  ElementRef,
  EventEmitter,
  Inject,
  InjectionToken,
  Input,
  NgModule,
  NgZone,
  Optional,
  Output,
  PLATFORM_ID,
  forwardRef,
  setClassMetadata,
  ɵɵInheritDefinitionFeature,
  ɵɵProvidersFeature,
  ɵɵStandaloneFeature,
  ɵɵdefineComponent,
  ɵɵdefineDirective,
  ɵɵdefineInjector,
  ɵɵdefineNgModule,
  ɵɵdirectiveInject,
  ɵɵtemplate
} from "./chunk-KXXOWBCK.js";
import {
  Subject,
  defer,
  fromEvent,
  mapTo,
  shareReplay,
  take,
  takeUntil
} from "./chunk-WSA2QMXP.js";
import {
  __spreadProps,
  __spreadValues
} from "./chunk-WKYGNSYM.js";

// node_modules/@tinymce/tinymce-angular/fesm2020/tinymce-tinymce-angular.mjs
function EditorComponent_ng_template_0_Template(rf, ctx) {
}
var getTinymce = () => {
  const w = typeof window !== "undefined" ? window : void 0;
  return w && w.tinymce ? w.tinymce : null;
};
var Events = class {
  constructor() {
    this.onBeforePaste = new EventEmitter();
    this.onBlur = new EventEmitter();
    this.onClick = new EventEmitter();
    this.onContextMenu = new EventEmitter();
    this.onCopy = new EventEmitter();
    this.onCut = new EventEmitter();
    this.onDblclick = new EventEmitter();
    this.onDrag = new EventEmitter();
    this.onDragDrop = new EventEmitter();
    this.onDragEnd = new EventEmitter();
    this.onDragGesture = new EventEmitter();
    this.onDragOver = new EventEmitter();
    this.onDrop = new EventEmitter();
    this.onFocus = new EventEmitter();
    this.onFocusIn = new EventEmitter();
    this.onFocusOut = new EventEmitter();
    this.onKeyDown = new EventEmitter();
    this.onKeyPress = new EventEmitter();
    this.onKeyUp = new EventEmitter();
    this.onMouseDown = new EventEmitter();
    this.onMouseEnter = new EventEmitter();
    this.onMouseLeave = new EventEmitter();
    this.onMouseMove = new EventEmitter();
    this.onMouseOut = new EventEmitter();
    this.onMouseOver = new EventEmitter();
    this.onMouseUp = new EventEmitter();
    this.onPaste = new EventEmitter();
    this.onSelectionChange = new EventEmitter();
    this.onActivate = new EventEmitter();
    this.onAddUndo = new EventEmitter();
    this.onBeforeAddUndo = new EventEmitter();
    this.onBeforeExecCommand = new EventEmitter();
    this.onBeforeGetContent = new EventEmitter();
    this.onBeforeRenderUI = new EventEmitter();
    this.onBeforeSetContent = new EventEmitter();
    this.onChange = new EventEmitter();
    this.onClearUndos = new EventEmitter();
    this.onDeactivate = new EventEmitter();
    this.onDirty = new EventEmitter();
    this.onExecCommand = new EventEmitter();
    this.onGetContent = new EventEmitter();
    this.onHide = new EventEmitter();
    this.onInit = new EventEmitter();
    this.onInitNgModel = new EventEmitter();
    this.onLoadContent = new EventEmitter();
    this.onNodeChange = new EventEmitter();
    this.onPostProcess = new EventEmitter();
    this.onPostRender = new EventEmitter();
    this.onPreInit = new EventEmitter();
    this.onPreProcess = new EventEmitter();
    this.onProgressState = new EventEmitter();
    this.onRedo = new EventEmitter();
    this.onRemove = new EventEmitter();
    this.onReset = new EventEmitter();
    this.onResizeEditor = new EventEmitter();
    this.onSaveContent = new EventEmitter();
    this.onSetAttrib = new EventEmitter();
    this.onObjectResizeStart = new EventEmitter();
    this.onObjectResized = new EventEmitter();
    this.onObjectSelected = new EventEmitter();
    this.onSetContent = new EventEmitter();
    this.onShow = new EventEmitter();
    this.onSubmit = new EventEmitter();
    this.onUndo = new EventEmitter();
    this.onVisualAid = new EventEmitter();
  }
};
Events.ɵfac = function Events_Factory(t) {
  return new (t || Events)();
};
Events.ɵdir = ɵɵdefineDirective({
  type: Events,
  outputs: {
    onBeforePaste: "onBeforePaste",
    onBlur: "onBlur",
    onClick: "onClick",
    onContextMenu: "onContextMenu",
    onCopy: "onCopy",
    onCut: "onCut",
    onDblclick: "onDblclick",
    onDrag: "onDrag",
    onDragDrop: "onDragDrop",
    onDragEnd: "onDragEnd",
    onDragGesture: "onDragGesture",
    onDragOver: "onDragOver",
    onDrop: "onDrop",
    onFocus: "onFocus",
    onFocusIn: "onFocusIn",
    onFocusOut: "onFocusOut",
    onKeyDown: "onKeyDown",
    onKeyPress: "onKeyPress",
    onKeyUp: "onKeyUp",
    onMouseDown: "onMouseDown",
    onMouseEnter: "onMouseEnter",
    onMouseLeave: "onMouseLeave",
    onMouseMove: "onMouseMove",
    onMouseOut: "onMouseOut",
    onMouseOver: "onMouseOver",
    onMouseUp: "onMouseUp",
    onPaste: "onPaste",
    onSelectionChange: "onSelectionChange",
    onActivate: "onActivate",
    onAddUndo: "onAddUndo",
    onBeforeAddUndo: "onBeforeAddUndo",
    onBeforeExecCommand: "onBeforeExecCommand",
    onBeforeGetContent: "onBeforeGetContent",
    onBeforeRenderUI: "onBeforeRenderUI",
    onBeforeSetContent: "onBeforeSetContent",
    onChange: "onChange",
    onClearUndos: "onClearUndos",
    onDeactivate: "onDeactivate",
    onDirty: "onDirty",
    onExecCommand: "onExecCommand",
    onGetContent: "onGetContent",
    onHide: "onHide",
    onInit: "onInit",
    onInitNgModel: "onInitNgModel",
    onLoadContent: "onLoadContent",
    onNodeChange: "onNodeChange",
    onPostProcess: "onPostProcess",
    onPostRender: "onPostRender",
    onPreInit: "onPreInit",
    onPreProcess: "onPreProcess",
    onProgressState: "onProgressState",
    onRedo: "onRedo",
    onRemove: "onRemove",
    onReset: "onReset",
    onResizeEditor: "onResizeEditor",
    onSaveContent: "onSaveContent",
    onSetAttrib: "onSetAttrib",
    onObjectResizeStart: "onObjectResizeStart",
    onObjectResized: "onObjectResized",
    onObjectSelected: "onObjectSelected",
    onSetContent: "onSetContent",
    onShow: "onShow",
    onSubmit: "onSubmit",
    onUndo: "onUndo",
    onVisualAid: "onVisualAid"
  }
});
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(Events, [{
    type: Directive
  }], null, {
    onBeforePaste: [{
      type: Output
    }],
    onBlur: [{
      type: Output
    }],
    onClick: [{
      type: Output
    }],
    onContextMenu: [{
      type: Output
    }],
    onCopy: [{
      type: Output
    }],
    onCut: [{
      type: Output
    }],
    onDblclick: [{
      type: Output
    }],
    onDrag: [{
      type: Output
    }],
    onDragDrop: [{
      type: Output
    }],
    onDragEnd: [{
      type: Output
    }],
    onDragGesture: [{
      type: Output
    }],
    onDragOver: [{
      type: Output
    }],
    onDrop: [{
      type: Output
    }],
    onFocus: [{
      type: Output
    }],
    onFocusIn: [{
      type: Output
    }],
    onFocusOut: [{
      type: Output
    }],
    onKeyDown: [{
      type: Output
    }],
    onKeyPress: [{
      type: Output
    }],
    onKeyUp: [{
      type: Output
    }],
    onMouseDown: [{
      type: Output
    }],
    onMouseEnter: [{
      type: Output
    }],
    onMouseLeave: [{
      type: Output
    }],
    onMouseMove: [{
      type: Output
    }],
    onMouseOut: [{
      type: Output
    }],
    onMouseOver: [{
      type: Output
    }],
    onMouseUp: [{
      type: Output
    }],
    onPaste: [{
      type: Output
    }],
    onSelectionChange: [{
      type: Output
    }],
    onActivate: [{
      type: Output
    }],
    onAddUndo: [{
      type: Output
    }],
    onBeforeAddUndo: [{
      type: Output
    }],
    onBeforeExecCommand: [{
      type: Output
    }],
    onBeforeGetContent: [{
      type: Output
    }],
    onBeforeRenderUI: [{
      type: Output
    }],
    onBeforeSetContent: [{
      type: Output
    }],
    onChange: [{
      type: Output
    }],
    onClearUndos: [{
      type: Output
    }],
    onDeactivate: [{
      type: Output
    }],
    onDirty: [{
      type: Output
    }],
    onExecCommand: [{
      type: Output
    }],
    onGetContent: [{
      type: Output
    }],
    onHide: [{
      type: Output
    }],
    onInit: [{
      type: Output
    }],
    onInitNgModel: [{
      type: Output
    }],
    onLoadContent: [{
      type: Output
    }],
    onNodeChange: [{
      type: Output
    }],
    onPostProcess: [{
      type: Output
    }],
    onPostRender: [{
      type: Output
    }],
    onPreInit: [{
      type: Output
    }],
    onPreProcess: [{
      type: Output
    }],
    onProgressState: [{
      type: Output
    }],
    onRedo: [{
      type: Output
    }],
    onRemove: [{
      type: Output
    }],
    onReset: [{
      type: Output
    }],
    onResizeEditor: [{
      type: Output
    }],
    onSaveContent: [{
      type: Output
    }],
    onSetAttrib: [{
      type: Output
    }],
    onObjectResizeStart: [{
      type: Output
    }],
    onObjectResized: [{
      type: Output
    }],
    onObjectSelected: [{
      type: Output
    }],
    onSetContent: [{
      type: Output
    }],
    onShow: [{
      type: Output
    }],
    onSubmit: [{
      type: Output
    }],
    onUndo: [{
      type: Output
    }],
    onVisualAid: [{
      type: Output
    }]
  });
})();
var validEvents = ["onActivate", "onAddUndo", "onBeforeAddUndo", "onBeforeExecCommand", "onBeforeGetContent", "onBeforeRenderUI", "onBeforeSetContent", "onBeforePaste", "onBlur", "onChange", "onClearUndos", "onClick", "onContextMenu", "onCopy", "onCut", "onDblclick", "onDeactivate", "onDirty", "onDrag", "onDragDrop", "onDragEnd", "onDragGesture", "onDragOver", "onDrop", "onExecCommand", "onFocus", "onFocusIn", "onFocusOut", "onGetContent", "onHide", "onInit", "onKeyDown", "onKeyPress", "onKeyUp", "onLoadContent", "onMouseDown", "onMouseEnter", "onMouseLeave", "onMouseMove", "onMouseOut", "onMouseOver", "onMouseUp", "onNodeChange", "onObjectResizeStart", "onObjectResized", "onObjectSelected", "onPaste", "onPostProcess", "onPostRender", "onPreProcess", "onProgressState", "onRedo", "onRemove", "onReset", "onResizeEditor", "onSaveContent", "onSelectionChange", "onSetAttrib", "onSetContent", "onShow", "onSubmit", "onUndo", "onVisualAid"];
var listenTinyMCEEvent = (editor, eventName, destroy$) => fromEvent(editor, eventName).pipe(takeUntil(destroy$));
var bindHandlers = (ctx, editor, destroy$) => {
  const allowedEvents = getValidEvents(ctx);
  allowedEvents.forEach((eventName) => {
    const eventEmitter = ctx[eventName];
    listenTinyMCEEvent(editor, eventName.substring(2), destroy$).subscribe((event) => {
      if (eventEmitter.observers.length > 0) {
        ctx.ngZone.run(() => eventEmitter.emit({
          event,
          editor
        }));
      }
    });
  });
};
var getValidEvents = (ctx) => {
  const ignoredEvents = parseStringProperty(ctx.ignoreEvents, []);
  const allowedEvents = parseStringProperty(ctx.allowedEvents, validEvents).filter((event) => validEvents.includes(event) && !ignoredEvents.includes(event));
  return allowedEvents;
};
var parseStringProperty = (property, defaultValue) => {
  if (typeof property === "string") {
    return property.split(",").map((value) => value.trim());
  }
  if (Array.isArray(property)) {
    return property;
  }
  return defaultValue;
};
var unique = 0;
var uuid = (prefix) => {
  const date = /* @__PURE__ */ new Date();
  const time = date.getTime();
  const random = Math.floor(Math.random() * 1e9);
  unique++;
  return prefix + "_" + random + unique + String(time);
};
var isTextarea = (element) => typeof element !== "undefined" && element.tagName.toLowerCase() === "textarea";
var normalizePluginArray = (plugins) => {
  if (typeof plugins === "undefined" || plugins === "") {
    return [];
  }
  return Array.isArray(plugins) ? plugins : plugins.split(" ");
};
var mergePlugins = (initPlugins, inputPlugins) => normalizePluginArray(initPlugins).concat(normalizePluginArray(inputPlugins));
var noop = () => {
};
var isNullOrUndefined = (value) => value === null || value === void 0;
var createState = () => ({
  script$: null
});
var CreateScriptLoader = () => {
  let state = createState();
  const load = (doc, url) => state.script$ || // Caretaker note: the `script$` is a multicast observable since it's piped with `shareReplay`,
  // so if there're multiple editor components simultaneously on the page, they'll subscribe to the internal
  // `ReplaySubject`. The script will be loaded only once, and `ReplaySubject` will cache the result.
  (state.script$ = defer(() => {
    const scriptTag = doc.createElement("script");
    scriptTag.referrerPolicy = "origin";
    scriptTag.type = "application/javascript";
    scriptTag.src = url;
    doc.head.appendChild(scriptTag);
    return fromEvent(scriptTag, "load").pipe(take(1), mapTo(void 0));
  }).pipe(shareReplay({
    bufferSize: 1,
    refCount: true
  })));
  const reinitialize = () => {
    state = createState();
  };
  return {
    load,
    reinitialize
  };
};
var ScriptLoader = CreateScriptLoader();
var TINYMCE_SCRIPT_SRC = new InjectionToken("TINYMCE_SCRIPT_SRC");
var EDITOR_COMPONENT_VALUE_ACCESSOR = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => EditorComponent),
  multi: true
};
var EditorComponent = class extends Events {
  constructor(elementRef, ngZone, platformId, tinymceScriptSrc) {
    super();
    this.platformId = platformId;
    this.tinymceScriptSrc = tinymceScriptSrc;
    this.cloudChannel = "6";
    this.apiKey = "no-api-key";
    this.id = "";
    this.modelEvents = "change input undo redo";
    this.onTouchedCallback = noop;
    this.destroy$ = new Subject();
    this.initialise = () => {
      const finalInit = __spreadProps(__spreadValues({}, this.init), {
        selector: void 0,
        target: this._element,
        inline: this.inline,
        readonly: this.disabled,
        plugins: mergePlugins(this.init && this.init.plugins, this.plugins),
        toolbar: this.toolbar || this.init && this.init.toolbar,
        setup: (editor) => {
          this._editor = editor;
          listenTinyMCEEvent(editor, "init", this.destroy$).subscribe(() => {
            this.initEditor(editor);
          });
          bindHandlers(this, editor, this.destroy$);
          if (this.init && typeof this.init.setup === "function") {
            this.init.setup(editor);
          }
        }
      });
      if (isTextarea(this._element)) {
        this._element.style.visibility = "";
      }
      this.ngZone.runOutsideAngular(() => {
        getTinymce().init(finalInit);
      });
    };
    this._elementRef = elementRef;
    this.ngZone = ngZone;
  }
  set disabled(val) {
    this._disabled = val;
    if (this._editor && this._editor.initialized) {
      if (typeof this._editor.mode?.set === "function") {
        this._editor.mode.set(val ? "readonly" : "design");
      } else {
        this._editor.setMode(val ? "readonly" : "design");
      }
    }
  }
  get disabled() {
    return this._disabled;
  }
  get editor() {
    return this._editor;
  }
  writeValue(value) {
    if (this._editor && this._editor.initialized) {
      this._editor.setContent(isNullOrUndefined(value) ? "" : value);
    } else {
      this.initialValue = value === null ? void 0 : value;
    }
  }
  registerOnChange(fn) {
    this.onChangeCallback = fn;
  }
  registerOnTouched(fn) {
    this.onTouchedCallback = fn;
  }
  setDisabledState(isDisabled) {
    this.disabled = isDisabled;
  }
  ngAfterViewInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.id = this.id || uuid("tiny-angular");
      this.inline = this.inline !== void 0 ? this.inline !== false : !!this.init?.inline;
      this.createElement();
      if (getTinymce() !== null) {
        this.initialise();
      } else if (this._element && this._element.ownerDocument) {
        ScriptLoader.load(this._element.ownerDocument, this.getScriptSrc()).pipe(takeUntil(this.destroy$)).subscribe(this.initialise);
      }
    }
  }
  ngOnDestroy() {
    this.destroy$.next();
    if (getTinymce() !== null) {
      getTinymce().remove(this._editor);
    }
  }
  createElement() {
    const tagName = typeof this.tagName === "string" ? this.tagName : "div";
    this._element = document.createElement(this.inline ? tagName : "textarea");
    if (this._element) {
      if (document.getElementById(this.id)) {
        console.warn(`TinyMCE-Angular: an element with id [${this.id}] already exists. Editors with duplicate Id will not be able to mount`);
      }
      this._element.id = this.id;
      if (isTextarea(this._element)) {
        this._element.style.visibility = "hidden";
      }
      this._elementRef.nativeElement.appendChild(this._element);
    }
  }
  getScriptSrc() {
    return isNullOrUndefined(this.tinymceScriptSrc) ? `https://cdn.tiny.cloud/1/${this.apiKey}/tinymce/${this.cloudChannel}/tinymce.min.js` : this.tinymceScriptSrc;
  }
  initEditor(editor) {
    listenTinyMCEEvent(editor, "blur", this.destroy$).subscribe(() => {
      this.ngZone.run(() => this.onTouchedCallback());
    });
    listenTinyMCEEvent(editor, this.modelEvents, this.destroy$).subscribe(() => {
      this.ngZone.run(() => this.emitOnChange(editor));
    });
    if (typeof this.initialValue === "string") {
      this.ngZone.run(() => {
        editor.setContent(this.initialValue);
        if (editor.getContent() !== this.initialValue) {
          this.emitOnChange(editor);
        }
        if (this.onInitNgModel !== void 0) {
          this.onInitNgModel.emit(editor);
        }
      });
    }
  }
  emitOnChange(editor) {
    if (this.onChangeCallback) {
      this.onChangeCallback(editor.getContent({
        format: this.outputFormat
      }));
    }
  }
};
EditorComponent.ɵfac = function EditorComponent_Factory(t) {
  return new (t || EditorComponent)(ɵɵdirectiveInject(ElementRef), ɵɵdirectiveInject(NgZone), ɵɵdirectiveInject(PLATFORM_ID), ɵɵdirectiveInject(TINYMCE_SCRIPT_SRC, 8));
};
EditorComponent.ɵcmp = ɵɵdefineComponent({
  type: EditorComponent,
  selectors: [["editor"]],
  inputs: {
    cloudChannel: "cloudChannel",
    apiKey: "apiKey",
    init: "init",
    id: "id",
    initialValue: "initialValue",
    outputFormat: "outputFormat",
    inline: "inline",
    tagName: "tagName",
    plugins: "plugins",
    toolbar: "toolbar",
    modelEvents: "modelEvents",
    allowedEvents: "allowedEvents",
    ignoreEvents: "ignoreEvents",
    disabled: "disabled"
  },
  standalone: true,
  features: [ɵɵProvidersFeature([EDITOR_COMPONENT_VALUE_ACCESSOR]), ɵɵInheritDefinitionFeature, ɵɵStandaloneFeature],
  decls: 1,
  vars: 0,
  template: function EditorComponent_Template(rf, ctx) {
    if (rf & 1) {
      ɵɵtemplate(0, EditorComponent_ng_template_0_Template, 0, 0, "ng-template");
    }
  },
  dependencies: [CommonModule, FormsModule],
  styles: ["[_nghost-%COMP%]{display:block}"]
});
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(EditorComponent, [{
    type: Component,
    args: [{
      selector: "editor",
      template: "<ng-template></ng-template>",
      providers: [EDITOR_COMPONENT_VALUE_ACCESSOR],
      standalone: true,
      imports: [CommonModule, FormsModule],
      styles: [":host{display:block}\n"]
    }]
  }], function() {
    return [{
      type: ElementRef
    }, {
      type: NgZone
    }, {
      type: Object,
      decorators: [{
        type: Inject,
        args: [PLATFORM_ID]
      }]
    }, {
      type: void 0,
      decorators: [{
        type: Optional
      }, {
        type: Inject,
        args: [TINYMCE_SCRIPT_SRC]
      }]
    }];
  }, {
    cloudChannel: [{
      type: Input
    }],
    apiKey: [{
      type: Input
    }],
    init: [{
      type: Input
    }],
    id: [{
      type: Input
    }],
    initialValue: [{
      type: Input
    }],
    outputFormat: [{
      type: Input
    }],
    inline: [{
      type: Input
    }],
    tagName: [{
      type: Input
    }],
    plugins: [{
      type: Input
    }],
    toolbar: [{
      type: Input
    }],
    modelEvents: [{
      type: Input
    }],
    allowedEvents: [{
      type: Input
    }],
    ignoreEvents: [{
      type: Input
    }],
    disabled: [{
      type: Input
    }]
  });
})();
var EditorModule = class {
};
EditorModule.ɵfac = function EditorModule_Factory(t) {
  return new (t || EditorModule)();
};
EditorModule.ɵmod = ɵɵdefineNgModule({
  type: EditorModule,
  imports: [EditorComponent],
  exports: [EditorComponent]
});
EditorModule.ɵinj = ɵɵdefineInjector({
  imports: [EditorComponent]
});
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(EditorModule, [{
    type: NgModule,
    args: [{
      imports: [EditorComponent],
      exports: [EditorComponent]
    }]
  }], null, null);
})();
export {
  EditorComponent,
  EditorModule,
  TINYMCE_SCRIPT_SRC
};
//# sourceMappingURL=@tinymce_tinymce-angular.js.map
