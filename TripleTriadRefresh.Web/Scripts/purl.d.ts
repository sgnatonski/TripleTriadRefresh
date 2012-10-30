// Interface
interface Purl {
    data: string;
    attr(attr: string): string;			
    param(param:string): string;
	fparam(param:string): string;
	segment(seg: string): string;
	fsegment(seg: string): string;
}

// extend JQuery interface
interface JQueryStatic {
    url(url?: string): Purl;
}