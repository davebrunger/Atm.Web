export type DatesAsStrings<T> =
    T extends Date ? string : (
        T extends Array<infer U> ? DatesAsStrings<U>[] : (
            T extends Object ? { [K in keyof T]: DatesAsStrings<T[K]> } : T
        )
    );