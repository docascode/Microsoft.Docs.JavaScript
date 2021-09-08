﻿using System;
using System.Runtime.InteropServices;

namespace Microsoft.Docs.Build
{
    internal delegate void JsRun(IntPtr scope);
    internal delegate void JsResult(IntPtr scope, IntPtr value);

    internal static unsafe class NativeMethods
    {
        private const string LibName = "docfxjsv8";

        [DllImport(LibName)]
        public static extern JavaScriptValueType js_value_type(IntPtr value);

        [DllImport(LibName)]
        public static extern IntPtr js_undefined(IntPtr scope);

        [DllImport(LibName)]
        public static extern IntPtr js_null(IntPtr scope);

        [DllImport(LibName)]
        public static extern IntPtr js_true(IntPtr scope);

        [DllImport(LibName)]
        public static extern IntPtr js_false(IntPtr scope);

        [DllImport(LibName)]
        public static extern IntPtr js_integer_new(IntPtr scope, int value);

        [DllImport(LibName)]
        public static extern long js_integer_value(IntPtr scope, IntPtr value);

        [DllImport(LibName)]
        public static extern IntPtr js_number_new(IntPtr scope, double value);

        [DllImport(LibName)]
        public static extern double js_number_value(IntPtr scope, IntPtr value);

        [DllImport(LibName)]
        public static extern IntPtr js_string_new(IntPtr scope, char* chars, nint length);

        [DllImport(LibName)]
        public static extern nint js_string_length(IntPtr value);

        [DllImport(LibName)]
        public static extern nint js_string_copy(IntPtr scope, IntPtr value, char* buffer, nint length);

        [DllImport(LibName)]
        public static extern IntPtr js_array_new(IntPtr scope, int length);

        [DllImport(LibName)]
        public static extern uint js_array_length(IntPtr array);

        [DllImport(LibName)]
        public static extern IntPtr js_array_get_index(IntPtr scope, IntPtr array, uint index);

        [DllImport(LibName)]
        public static extern IntPtr js_array_set_index(IntPtr scope, IntPtr array, uint index, IntPtr value);

        [DllImport(LibName)]
        public static extern IntPtr js_object_new(IntPtr scope);

        [DllImport(LibName)]
        public static extern IntPtr js_object_get_own_property_names(IntPtr scope, IntPtr obj);

        [DllImport(LibName)]
        public static extern IntPtr js_object_get_property(IntPtr scope, IntPtr obj, IntPtr key);

        [DllImport(LibName)]
        public static extern void js_object_set_property(IntPtr scope, IntPtr obj, IntPtr key, IntPtr value);

        [DllImport(LibName)]
        public static extern void js_function_call(IntPtr scope, IntPtr value, IntPtr recv, IntPtr* argv, nint argc, JsResult error, JsResult result);

        [DllImport(LibName)]
        public static extern IntPtr js_isolate_new();

        [DllImport(LibName)]
        public static extern void js_isolate_delete(IntPtr isolate);

        [DllImport(LibName)]
        public static extern void js_run_in_context(IntPtr isolate, JsRun callback);

        [DllImport(LibName)]
        public static extern void js_run_script(IntPtr scope, IntPtr code, IntPtr filename, JsResult error, JsResult result);

        public static IntPtr ToJsString(IntPtr scope, string value)
        {
            fixed (char* chars = value)
            {
                return js_string_new(scope, chars, value.Length);
            }
        }

        public static string FromJsString(IntPtr scope, IntPtr str)
        {
            var length = js_string_length(str);

            return string.Create((int)length, (scope, str, length), (buffer, state) =>
            {
                fixed (char* buf = buffer)
                {
                    js_string_copy(state.scope, state.str, buf, state.length);
                }
            });
        }
    }
}
