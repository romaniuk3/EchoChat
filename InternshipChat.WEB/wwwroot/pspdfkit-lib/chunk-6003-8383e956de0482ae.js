/*!
 * PSPDFKit for Web 2023.3.1 (https://pspdfkit.com/web)
 *
 * Copyright (c) 2016-2023 PSPDFKit GmbH. All rights reserved.
 *
 * THIS SOURCE CODE AND ANY ACCOMPANYING DOCUMENTATION ARE PROTECTED BY INTERNATIONAL COPYRIGHT LAW
 * AND MAY NOT BE RESOLD OR REDISTRIBUTED. USAGE IS BOUND TO THE PSPDFKIT LICENSE AGREEMENT.
 * UNAUTHORIZED REPRODUCTION OR DISTRIBUTION IS SUBJECT TO CIVIL AND CRIMINAL PENALTIES.
 * This notice may not be removed from this file.
 *
 * PSPDFKit uses several open source third-party components: https://pspdfkit.com/acknowledgements/web/
 */
"use strict";(globalThis.webpackChunkPSPDFKit=globalThis.webpackChunkPSPDFKit||[]).push([[6003],{26003:(e,a,t)=>{t.r(a),t.d(a,{default:()=>p});var r=t(35712);const d="pspdfkit-lib/gdpicture-591942f5f8ea6234924cd89562633fff45b1cbf6",i=`${d}/initDotnet.js`;let s;!function(e){e[e.pdf_a_1a=0]="pdf_a_1a",e[e.pdf_a_1b=1]="pdf_a_1b",e[e.pdf_a_2a=2]="pdf_a_2a",e[e.pdf_a_2u=3]="pdf_a_2u",e[e.pdf_a_2b=4]="pdf_a_2b",e[e.pdf_a_3a=5]="pdf_a_3a",e[e.pdf_a_3u=6]="pdf_a_3u",e[e.pdf_a_3b=7]="pdf_a_3b",e[e.pdf_a_4=8]="pdf_a_4",e[e.pdf_a_4e=9]="pdf_a_4e",e[e.pdf_a_4f=10]="pdf_a_4f"}(s||(s={}));const n="/create.pdf",f="/save.pdf";let o=null,_=null;function c(e){let a;(0,r.kG)(_,"GdPicture WebAssembly is not loaded.");for(var t=arguments.length,d=new Array(t>1?t-1:0),i=1;i<t;i++)d[i-1]=arguments[i];for(const e of d)a=Object.assign(d[0],e);const s=JSON.stringify({type:e,...a}),n=JSON.parse(_.CommandHandler(s));if(!n.success)throw new Error(n.errorReason+": "+n.errorMessage+"\n"+n.error);return n}class l{async loadModule(e,a,t){const{Assemblies:r,Module:s}=await async function(e){const{initDotnet:a}=await import(`${e}${i}`);return a(`${e}${d}`)}(e);_=r.GdPictureWasm.API,o=s,c("gdpicture/setLicense",{origin:a},{licenseKey:t||"DEMO_PSPDFKIT_WEB"})}toPdf(e,a){o.FS.writeFile(n,new Uint8Array(e));const t={file:f};a&&a in s&&(t.conformance=a);try{return c("gdpicture/process",{input:{file:n},output:t}),o.FS.readFile(f).buffer}finally{try{o.FS.unlink(f)}catch(e){}}}}const p=class{constructor(e){let{baseUrl:a,mainThreadOrigin:t,licenseKey:r}=e;this.gdPicture=new l,this.moduleLoadPromise=this.gdPicture.loadModule(a,t,r)}async toPdf(e,a){let t;return this.moduleLoadPromise&&await this.moduleLoadPromise,a&&(t=a.replace("pdf","pdf_").replaceAll("-","_")),this.gdPicture.toPdf(e,t)}destroy(){}}}}]);