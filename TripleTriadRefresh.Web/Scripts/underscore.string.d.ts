/// <reference path="underscore.browser.d.ts" />
module underscore.string {
    export interface UnderscoreStringStatic {

        isBlank(str: string): bool;

        /*stripTags(str: string): string;
    
        capitalize(str: string): string;
    
        chop(str: string, step: number): string[];
    
        clean(str: string): string;
    
        count(str: string, substr: string): number;
    
        chars(str: string): string[];
    
        swapCase(str: string): string;
    
        escapeHTML(str: string): string;
    
        unescapeHTML(str: string): string;
    
        escapeRegExp(str: string): string;
    
        splice(str: string, i, howmany, substr): string;
    
        insert(str: string, i, substr): string;
    
        include(str: string, needle): bool;
    
        join(...str: string[]): string;
    
        lines(str: string): string[];
    
        reverse(str: string): string;
    
        startsWith(str: string, starts: string): bool;
    
        endsWith(str: string, ends: string): bool;
    
        succ(str: string): string;
    
        titleize(str: string): string;
    
        camelize(str: string): string;
    
        underscored(str: string): string;
    
        dasherize(str: string): string;
    
        classify(str: string): string;
    
        humanize(str: string): string;
    
        trim(str: string, characters: string[]): string;
    
        ltrim(str: string, characters: string[]): string;
    
        rtrim(str: string, characters: string[]): string;
    
        truncate(str: string, length, truncateStr): string;
    
        prune(str: string, length, pruneStr): string;
    
        words(str: string, delimiter): string[];
    
        pad(str: string, length, padStr, type): string;
    
        lpad(str: string, length, padStr): string;
    
        rpad(str: string, length, padStr): string;
    
        lrpad(str: string, length, padStr): string;
    
        sprintf(fmt: string, ...args: any[]): string;
    
        vsprintf(fmt: string, ...args: any[]): string;
    
        toNumber(str: string, decimals): number;
    
        numberFormat(no: number, dec, dsep, tsep): string;
    
        strRight(str: string, sep): string;
    
        strRightBack(str: string, sep): string;
    
        strLeft(str: string, sep): string;
    
        strLeftBack(str: string, sep): string;
    
        toSentence(array, separator, lastSeparator, serial): string;
    
        toSentenceSerial(): string;
    
        slugify(str: string): string;
    
        surround(str: string, wrapper): string;
    
        quote(str: string): string;
    
        repeat(str: string, qty, separator): string;
    
        levenshtein(str1: string, str2: string): number;*/
    }
}

interface UnderscoreStatic {
    str: underscore.string.UnderscoreStringStatic;
    string: underscore.string.UnderscoreStringStatic;
}

var _s: underscore.string.UnderscoreStringStatic;