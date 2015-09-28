using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Borogove.Model
{
    public enum Role
    {
        /// <summary>Abridger</summary>
        abr,
        /// <summary>Art copyist</summary>
        acp,
        /// <summary>Actor</summary>
        act,
        /// <summary>Art director</summary>
        adi,
        /// <summary>Adapter</summary>
        adp,
        /// <summary>Author of afterword, colophon, etc.</summary>
        aft,
        /// <summary>Analyst</summary>
        anl,
        /// <summary>Animator</summary>
        anm,
        /// <summary>Annotator</summary>
        ann,
        /// <summary>Bibliographic antecedent</summary>
        ant,
        /// <summary>Appellee</summary>
        ape,
        /// <summary>Appellant</summary>
        apl,
        /// <summary>Applicant</summary>
        app,
        /// <summary>Author in quotations or text abstracts</summary>
        aqt,
        /// <summary>Architect</summary>
        arc,
        /// <summary>Artistic director</summary>
        ard,
        /// <summary>Arranger</summary>
        arr,
        /// <summary>Artist</summary>
        art,
        /// <summary>Assignee</summary>
        asg,
        /// <summary>Associated name</summary>
        asn,
        /// <summary>Autographer</summary>
        ato,
        /// <summary>Attributed name</summary>
        att,
        /// <summary>Auctioneer</summary>
        auc,
        /// <summary>Author of dialog</summary>
        aud,
        /// <summary>Author of introduction, etc.</summary>
        aui,
        /// <summary>Screenwriter</summary>
        aus,
        /// <summary>Author</summary>
        aut,
        /// <summary>Binding designer</summary>
        bdd,
        /// <summary>Bookjacket designer</summary>
        bjd,
        /// <summary>Book designer</summary>
        bkd,
        /// <summary>Book producer</summary>
        bkp,
        /// <summary>Blurb writer</summary>
        blw,
        /// <summary>Binder</summary>
        bnd,
        /// <summary>Bookplate designer</summary>
        bpd,
        /// <summary>Broadcaster</summary>
        brd,
        /// <summary>Braille embosser</summary>
        brl,
        /// <summary>Bookseller</summary>
        bsl,
        /// <summary>Caster</summary>
        cas,
        /// <summary>Conceptor</summary>
        ccp,
        /// <summary>Choreographer</summary>
        chr,
        /// <summary>   Collaborator</summary>
        clb,
        /// <summary>Client</summary>
        cli,
        /// <summary>Calligrapher</summary>
        cll,
        /// <summary>Colorist</summary>
        clr,
        /// <summary>Collotyper</summary>
        clt,
        /// <summary>Commentator</summary>
        cmm,
        /// <summary>Composer</summary>
        cmp,
        /// <summary>Compositor</summary>
        cmt,
        /// <summary>Conductor</summary>
        cnd,
        /// <summary>Cinematographer</summary>
        cng,
        /// <summary>Censor</summary>
        cns,
        /// <summary>Contestant-appellee</summary>
        coe,
        /// <summary>Collector</summary>
        col,
        /// <summary>Compiler</summary>
        com,
        /// <summary>Conservator</summary>
        con,
        /// <summary>Collection registrar</summary>
        cor,
        /// <summary>Contestant</summary>
        cos,
        /// <summary>Contestant-appellant</summary>
        cot,
        /// <summary>Court governed</summary>
        cou,
        /// <summary>Cover designer</summary>
        cov,
        /// <summary>Copyright claimant</summary>
        cpc,
        /// <summary>Complainant-appellee</summary>
        cpe,
        /// <summary>Copyright holder</summary>
        cph,
        /// <summary>Complainant</summary>
        cpl,
        /// <summary>Complainant-appellant</summary>
        cpt,
        /// <summary>Creator</summary>
        cre,
        /// <summary>Correspondent</summary>
        crp,
        /// <summary>Corrector</summary>
        crr,
        /// <summary>Court reporter</summary>
        crt,
        /// <summary>Consultant</summary>
        csl,
        /// <summary>Consultant to a project</summary>
        csp,
        /// <summary>Costume designer</summary>
        cst,
        /// <summary>Contributor</summary>
        ctb,
        /// <summary>Contestee-appellee</summary>
        cte,
        /// <summary>Cartographer</summary>
        ctg,
        /// <summary>Contractor</summary>
        ctr,
        /// <summary>Contestee</summary>
        cts,
        /// <summary>Contestee-appellant</summary>
        ctt,
        /// <summary>Curator</summary>
        cur,
        /// <summary>Commentator for written text</summary>
        cwt,
        /// <summary>Distribution place</summary>
        dbp,
        /// <summary>Defendant</summary>
        dfd,
        /// <summary>Defendant-appellee</summary>
        dfe,
        /// <summary>Defendant-appellant</summary>
        dft,
        /// <summary>Degree granting institution</summary>
        dgg,
        /// <summary>Degree supervisor</summary>
        dgs,
        /// <summary>Dissertant</summary>
        dis,
        /// <summary>Delineator</summary>
        dln,
        /// <summary>Dancer</summary>
        dnc,
        /// <summary>Donor</summary>
        dnr,
        /// <summary>Depicted</summary>
        dpc,
        /// <summary>Depositor</summary>
        dpt,
        /// <summary>Draftsman</summary>
        drm,
        /// <summary>Director</summary>
        drt,
        /// <summary>Designer</summary>
        dsr,
        /// <summary>Distributor</summary>
        dst,
        /// <summary>Data contributor</summary>
        dtc,
        /// <summary>Dedicatee</summary>
        dte,
        /// <summary>Data manager</summary>
        dtm,
        /// <summary>Dedicator</summary>
        dto,
        /// <summary>Dubious author</summary>
        dub,
        /// <summary>Editor of compilation</summary>
        edc,
        /// <summary>Editor of moving image work</summary>
        edm,
        /// <summary>Editor</summary>
        edt,
        /// <summary>Engraver</summary>
        egr,
        /// <summary>Electrician</summary>
        elg,
        /// <summary>Electrotyper</summary>
        elt,
        /// <summary>Engineer</summary>
        eng,
        /// <summary>Enacting jurisdiction</summary>
        enj,
        /// <summary>Etcher</summary>
        etr,
        /// <summary>Event place</summary>
        evp,
        /// <summary>Expert</summary>
        exp,
        /// <summary>Facsimilist</summary>
        fac,
        /// <summary>Film distributor</summary>
        fds,
        /// <summary>Field director</summary>
        fld,
        /// <summary>Film editor</summary>
        flm,
        /// <summary>Film director</summary>
        fmd,
        /// <summary>Filmmaker</summary>
        fmk,
        /// <summary>Former owner</summary>
        fmo,
        /// <summary>Film producer</summary>
        fmp,
        /// <summary>Funder</summary>
        fnd,
        /// <summary>First party</summary>
        fpy,
        /// <summary>Forger</summary>
        frg,
        /// <summary>Geographic information specialist</summary>
        gis,
        /// <summary>Host institution</summary>
        his,
        /// <summary>Honoree</summary>
        hnr,
        /// <summary>Host</summary>
        hst,
        /// <summary>Illustrator</summary>
        ill,
        /// <summary>Illuminator</summary>
        ilu,
        /// <summary>Inscriber</summary>
        ins,
        /// <summary>Inventor</summary>
        inv,
        /// <summary>Issuing body</summary>
        isb,
        /// <summary>Instrumentalist</summary>
        itr,
        /// <summary>Interviewee</summary>
        ive,
        /// <summary>Interviewer</summary>
        ivr,
        /// <summary>Judge</summary>
        jud,
        /// <summary>Jurisdiction governed</summary>
        jug,
        /// <summary>Laboratory</summary>
        lbr,
        /// <summary>Librettist</summary>
        lbt,
        /// <summary>Laboratory director</summary>
        ldr,
        /// <summary>Lead</summary>
        led,
        /// <summary>Libelee-appellee</summary>
        lee,
        /// <summary>Libelee</summary>
        lel,
        /// <summary>Lender</summary>
        len,
        /// <summary>Libelee-appellant</summary>
        let,
        /// <summary>Lighting designer</summary>
        lgd,
        /// <summary>Libelant-appellee</summary>
        lie,
        /// <summary>Libelant</summary>
        lil,
        /// <summary>Libelant-appellant</summary>
        lit,
        /// <summary>Landscape architect</summary>
        lsa,
        /// <summary>Licensee</summary>
        lse,
        /// <summary>Licensor</summary>
        lso,
        /// <summary>Lithographer</summary>
        ltg,
        /// <summary>Lyricist</summary>
        lyr,
        /// <summary>Music copyist</summary>
        mcp,
        /// <summary>Metadata contact</summary>
        mdc,
        /// <summary>Medium</summary>
        med,
        /// <summary>Manufacture place</summary>
        mfp,
        /// <summary>Manufacturer</summary>
        mfr,
        /// <summary>Moderator</summary>
        mod,
        /// <summary>Monitor</summary>
        mon,
        /// <summary>Marbler</summary>
        mrb,
        /// <summary>Markup editor</summary>
        mrk,
        /// <summary>Musical director</summary>
        msd,
        /// <summary>Metal-engraver</summary>
        mte,
        /// <summary>Minute taker</summary>
        mtk,
        /// <summary>Musician</summary>
        mus,
        /// <summary>Narrator</summary>
        nrt,
        /// <summary>Opponent</summary>
        opn,
        /// <summary>Originator</summary>
        org,
        /// <summary>Organizer</summary>
        orm,
        /// <summary>Onscreen presenter</summary>
        osp,
        /// <summary>Other</summary>
        oth,
        /// <summary>Owner</summary>
        own,
        /// <summary>Panelist</summary>
        pan,
        /// <summary>Patron</summary>
        pat,
        /// <summary>Publishing director</summary>
        pbd,
        /// <summary>Publisher</summary>
        pbl,
        /// <summary>Project director</summary>
        pdr,
        /// <summary>Proofreader</summary>
        pfr,
        /// <summary>Photographer</summary>
        pht,
        /// <summary>Platemaker</summary>
        plt,
        /// <summary>Permitting agency</summary>
        pma,
        /// <summary>Production manager</summary>
        pmn,
        /// <summary>Printer of plates</summary>
        pop,
        /// <summary>Papermaker</summary>
        ppm,
        /// <summary>Puppeteer</summary>
        ppt,
        /// <summary>Praeses</summary>
        pra,
        /// <summary>Process contact</summary>
        prc,
        /// <summary>Production personnel</summary>
        prd,
        /// <summary>Presenter</summary>
        pre,
        /// <summary>Performer</summary>
        prf,
        /// <summary>Programmer</summary>
        prg,
        /// <summary>Printmaker</summary>
        prm,
        /// <summary>Production company</summary>
        prn,
        /// <summary>Producer</summary>
        pro,
        /// <summary>Production place</summary>
        prp,
        /// <summary>Production designer</summary>
        prs,
        /// <summary>Printer</summary>
        prt,
        /// <summary>Provider</summary>
        prv,
        /// <summary>Patent applicant</summary>
        pta,
        /// <summary>Plaintiff-appellee</summary>
        pte,
        /// <summary>Plaintiff</summary>
        ptf,
        /// <summary>Patent holder</summary>
        pth,
        /// <summary>Plaintiff-appellant</summary>
        ptt,
        /// <summary>Publication place</summary>
        pup,
        /// <summary>Rubricator</summary>
        rbr,
        /// <summary>Recordist</summary>
        rcd,
        /// <summary>Recording engineer</summary>
        rce,
        /// <summary>Addressee</summary>
        rcp,
        /// <summary>Radio director</summary>
        rdd,
        /// <summary>Redaktor</summary>
        red,
        /// <summary>Renderer</summary>
        ren,
        /// <summary>Researcher</summary>
        res,
        /// <summary>Reviewer</summary>
        rev,
        /// <summary>Radio producer</summary>
        rpc,
        /// <summary>Repository</summary>
        rps,
        /// <summary>Reporter</summary>
        rpt,
        /// <summary>Responsible party</summary>
        rpy,
        /// <summary>Respondent-appellee</summary>
        rse,
        /// <summary>Restager</summary>
        rsg,
        /// <summary>Respondent</summary>
        rsp,
        /// <summary>Restorationist</summary>
        rsr,
        /// <summary>Respondent-appellant</summary>
        rst,
        /// <summary>Research team head</summary>
        rth,
        /// <summary>Research team member</summary>
        rtm,
        /// <summary>Scientific advisor</summary>
        sad,
        /// <summary>Scenarist</summary>
        sce,
        /// <summary>Sculptor</summary>
        scl,
        /// <summary>Scribe</summary>
        scr,
        /// <summary>Sound designer</summary>
        sds,
        /// <summary>Secretary</summary>
        sec,
        /// <summary>Stage director</summary>
        sgd,
        /// <summary>Signer</summary>
        sgn,
        /// <summary>Supporting host</summary>
        sht,
        /// <summary>Seller</summary>
        sll,
        /// <summary>Singer</summary>
        sng,
        /// <summary>Speaker</summary>
        spk,
        /// <summary>Sponsor</summary>
        spn,
        /// <summary>Second party</summary>
        spy,
        /// <summary>Surveyor</summary>
        srv,
        /// <summary>Set designer</summary>
        std,
        /// <summary>Setting</summary>
        stg,
        /// <summary>Storyteller</summary>
        stl,
        /// <summary>Stage manager</summary>
        stm,
        /// <summary>Standards body</summary>
        stn,
        /// <summary>Stereotyper</summary>
        str,
        /// <summary>Technical director</summary>
        tcd,
        /// <summary>Teacher</summary>
        tch,
        /// <summary>Thesis advisor</summary>
        ths,
        /// <summary>Television director</summary>
        tld,
        /// <summary>Television producer</summary>
        tlp,
        /// <summary>Transcriber</summary>
        trc,
        /// <summary>Translator</summary>
        trl,
        /// <summary>Type designer</summary>
        tyd,
        /// <summary>Typographer</summary>
        tyg,
        /// <summary>University place</summary>
        uvp,
        /// <summary>Voice actor</summary>
        vac,
        /// <summary>Videographer</summary>
        vdg,
        /// <summary>Writer of added commentary</summary>
        wac,
        /// <summary>Writer of added lyrics</summary>
        wal,
        /// <summary>Writer of accompanying material</summary>
        wam,
        /// <summary>Writer of added text</summary>
        wat,
        /// <summary>Woodcutter</summary>
        wdc,
        /// <summary>Wood engraver</summary>
        wde,
        /// <summary>Writer of introduction</summary>
        win,
        /// <summary>Witness</summary>
        wit,
        /// <summary>Writer of preface</summary>
        wpr,
        /// <summary>Writer of supplementary textual content</summary>
        wst,
    }

    public static class RoleUtilities
    {
        private readonly static Dictionary<Role, string> _roleToFriendlyNameDictionary = new Dictionary<Role, string>()
        {
#region Role To Friendly Definitions
            { Role.abr, @"abridger" },
            { Role.acp, @"art copyist" },
            { Role.act, @"actor" },
            { Role.adi, @"art director" },
            { Role.adp, @"adapter" },
            { Role.aft, @"author of afterword, colophon, etc." },
            { Role.anl, @"analyst" },
            { Role.anm, @"animator" },
            { Role.ann, @"annotator" },
            { Role.ant, @"bibliographic antecedent" },
            { Role.ape, @"appellee" },
            { Role.apl, @"appellant" },
            { Role.app, @"applicant" },
            { Role.aqt, @"author in quotations or text abstracts" },
            { Role.arc, @"architect" },
            { Role.ard, @"artistic director" },
            { Role.arr, @"arranger" },
            { Role.art, @"artist" },
            { Role.asg, @"assignee" },
            { Role.asn, @"associated name" },
            { Role.ato, @"autographer" },
            { Role.att, @"attributed name" },
            { Role.auc, @"auctioneer" },
            { Role.aud, @"author of dialog" },
            { Role.aui, @"author of introduction, etc." },
            { Role.aus, @"screenwriter" },
            { Role.aut, @"author" },
            { Role.bdd, @"binding designer" },
            { Role.bjd, @"bookjacket designer" },
            { Role.bkd, @"book designer" },
            { Role.bkp, @"book producer" },
            { Role.blw, @"blurb writer" },
            { Role.bnd, @"binder" },
            { Role.bpd, @"bookplate designer" },
            { Role.brd, @"broadcaster" },
            { Role.brl, @"braille embosser" },
            { Role.bsl, @"bookseller" },
            { Role.cas, @"caster" },
            { Role.ccp, @"conceptor" },
            { Role.chr, @"choreographer" },
            { Role.cli, @"client" },
            { Role.cll, @"calligrapher" },
            { Role.clr, @"colorist" },
            { Role.clt, @"collotyper" },
            { Role.cmm, @"commentator" },
            { Role.cmp, @"composer" },
            { Role.cmt, @"compositor" },
            { Role.cnd, @"conductor" },
            { Role.cng, @"cinematographer" },
            { Role.cns, @"censor" },
            { Role.coe, @"contestant-appellee" },
            { Role.col, @"collector" },
            { Role.com, @"compiler" },
            { Role.con, @"conservator" },
            { Role.cor, @"collection registrar" },
            { Role.cos, @"contestant" },
            { Role.cot, @"contestant-appellant" },
            { Role.cou, @"court governed" },
            { Role.cov, @"cover designer" },
            { Role.cpc, @"copyright claimant" },
            { Role.cpe, @"complainant-appellee" },
            { Role.cph, @"copyright holder" },
            { Role.cpl, @"complainant" },
            { Role.cpt, @"complainant-appellant" },
            { Role.cre, @"creator" },
            { Role.crp, @"correspondent" },
            { Role.crr, @"corrector" },
            { Role.crt, @"court reporter" },
            { Role.csl, @"consultant" },
            { Role.csp, @"consultant to a project" },
            { Role.cst, @"costume designer" },
            { Role.ctb, @"contributor" },
            { Role.cte, @"contestee-appellee" },
            { Role.ctg, @"cartographer" },
            { Role.ctr, @"contractor" },
            { Role.cts, @"contestee" },
            { Role.ctt, @"contestee-appellant" },
            { Role.cur, @"curator" },
            { Role.cwt, @"commentator for written text" },
            { Role.dbp, @"distribution place" },
            { Role.dfd, @"defendant" },
            { Role.dfe, @"defendant-appellee" },
            { Role.dft, @"defendant-appellant" },
            { Role.dgg, @"degree granting institution" },
            { Role.dgs, @"degree supervisor" },
            { Role.dis, @"dissertant" },
            { Role.dln, @"delineator" },
            { Role.dnc, @"dancer" },
            { Role.dnr, @"donor" },
            { Role.dpc, @"depicted" },
            { Role.dpt, @"depositor" },
            { Role.drm, @"draftsman" },
            { Role.drt, @"director" },
            { Role.dsr, @"designer" },
            { Role.dst, @"distributor" },
            { Role.dtc, @"data contributor" },
            { Role.dte, @"dedicatee" },
            { Role.dtm, @"data manager" },
            { Role.dto, @"dedicator" },
            { Role.dub, @"dubious author" },
            { Role.edc, @"editor of compilation" },
            { Role.edm, @"editor of moving image work" },
            { Role.edt, @"editor" },
            { Role.egr, @"engraver" },
            { Role.elg, @"electrician" },
            { Role.elt, @"electrotyper" },
            { Role.eng, @"engineer" },
            { Role.enj, @"enacting jurisdiction" },
            { Role.etr, @"etcher" },
            { Role.evp, @"event place" },
            { Role.exp, @"expert" },
            { Role.fac, @"facsimilist" },
            { Role.fds, @"film distributor" },
            { Role.fld, @"field director" },
            { Role.flm, @"film editor" },
            { Role.fmd, @"film director" },
            { Role.fmk, @"filmmaker" },
            { Role.fmo, @"former owner" },
            { Role.fmp, @"film producer" },
            { Role.fnd, @"funder" },
            { Role.fpy, @"first party" },
            { Role.frg, @"forger" },
            { Role.gis, @"geographic information specialist" },
            { Role.his, @"host institution" },
            { Role.hnr, @"honoree" },
            { Role.hst, @"host" },
            { Role.ill, @"illustrator" },
            { Role.ilu, @"illuminator" },
            { Role.ins, @"inscriber" },
            { Role.inv, @"inventor" },
            { Role.isb, @"issuing body" },
            { Role.itr, @"instrumentalist" },
            { Role.ive, @"interviewee" },
            { Role.ivr, @"interviewer" },
            { Role.jud, @"judge" },
            { Role.jug, @"jurisdiction governed" },
            { Role.lbr, @"laboratory" },
            { Role.lbt, @"librettist" },
            { Role.ldr, @"laboratory director" },
            { Role.led, @"lead" },
            { Role.lee, @"libelee-appellee" },
            { Role.lel, @"libelee" },
            { Role.len, @"lender" },
            { Role.let, @"libelee-appellant" },
            { Role.lgd, @"lighting designer" },
            { Role.lie, @"libelant-appellee" },
            { Role.lil, @"libelant" },
            { Role.lit, @"libelant-appellant" },
            { Role.lsa, @"landscape architect" },
            { Role.lse, @"licensee" },
            { Role.lso, @"licensor" },
            { Role.ltg, @"lithographer" },
            { Role.lyr, @"lyricist" },
            { Role.mcp, @"music copyist" },
            { Role.mdc, @"metadata contact" },
            { Role.med, @"medium" },
            { Role.mfp, @"manufacture place" },
            { Role.mfr, @"manufacturer" },
            { Role.mod, @"moderator" },
            { Role.mon, @"monitor" },
            { Role.mrb, @"marbler" },
            { Role.mrk, @"markup editor" },
            { Role.msd, @"musical director" },
            { Role.mte, @"metal-engraver" },
            { Role.mtk, @"minute taker" },
            { Role.mus, @"musician" },
            { Role.nrt, @"narrator" },
            { Role.opn, @"opponent" },
            { Role.org, @"originator" },
            { Role.orm, @"organizer" },
            { Role.osp, @"onscreen presenter" },
            { Role.oth, @"other" },
            { Role.own, @"owner" },
            { Role.pan, @"panelist" },
            { Role.pat, @"patron" },
            { Role.pbd, @"publishing director" },
            { Role.pbl, @"publisher" },
            { Role.pdr, @"project director" },
            { Role.pfr, @"proofreader" },
            { Role.pht, @"photographer" },
            { Role.plt, @"platemaker" },
            { Role.pma, @"permitting agency" },
            { Role.pmn, @"production manager" },
            { Role.pop, @"printer of plates" },
            { Role.ppm, @"papermaker" },
            { Role.ppt, @"puppeteer" },
            { Role.pra, @"praeses" },
            { Role.prc, @"process contact" },
            { Role.prd, @"production personnel" },
            { Role.pre, @"presenter" },
            { Role.prf, @"performer" },
            { Role.prg, @"programmer" },
            { Role.prm, @"printmaker" },
            { Role.prn, @"production company" },
            { Role.pro, @"producer" },
            { Role.prp, @"production place" },
            { Role.prs, @"production designer" },
            { Role.prt, @"printer" },
            { Role.prv, @"provider" },
            { Role.pta, @"patent applicant" },
            { Role.pte, @"plaintiff-appellee" },
            { Role.ptf, @"plaintiff" },
            { Role.pth, @"patent holder" },
            { Role.ptt, @"plaintiff-appellant" },
            { Role.pup, @"publication place" },
            { Role.rbr, @"rubricator" },
            { Role.rcd, @"recordist" },
            { Role.rce, @"recording engineer" },
            { Role.rcp, @"addressee" },
            { Role.rdd, @"radio director" },
            { Role.red, @"redaktor" },
            { Role.ren, @"renderer" },
            { Role.res, @"researcher" },
            { Role.rev, @"reviewer" },
            { Role.rpc, @"radio producer" },
            { Role.rps, @"repository" },
            { Role.rpt, @"reporter" },
            { Role.rpy, @"responsible party" },
            { Role.rse, @"respondent-appellee" },
            { Role.rsg, @"restager" },
            { Role.rsp, @"respondent" },
            { Role.rsr, @"restorationist" },
            { Role.rst, @"respondent-appellant" },
            { Role.rth, @"research team head" },
            { Role.rtm, @"research team member" },
            { Role.sad, @"scientific advisor" },
            { Role.sce, @"scenarist" },
            { Role.scl, @"sculptor" },
            { Role.scr, @"scribe" },
            { Role.sds, @"sound designer" },
            { Role.sec, @"secretary" },
            { Role.sgd, @"stage director" },
            { Role.sgn, @"signer" },
            { Role.sht, @"supporting host" },
            { Role.sll, @"seller" },
            { Role.sng, @"singer" },
            { Role.spk, @"speaker" },
            { Role.spn, @"sponsor" },
            { Role.spy, @"second party" },
            { Role.srv, @"surveyor" },
            { Role.std, @"set designer" },
            { Role.stg, @"setting" },
            { Role.stl, @"storyteller" },
            { Role.stm, @"stage manager" },
            { Role.stn, @"standards body" },
            { Role.str, @"stereotyper" },
            { Role.tcd, @"technical director" },
            { Role.tch, @"teacher" },
            { Role.ths, @"thesis advisor" },
            { Role.tld, @"television director" },
            { Role.tlp, @"television producer" },
            { Role.trc, @"transcriber" },
            { Role.trl, @"translator" },
            { Role.tyd, @"type designer" },
            { Role.tyg, @"typographer" },
            { Role.uvp, @"university place" },
            { Role.vac, @"voice actor" },
            { Role.vdg, @"videographer" },
            { Role.wac, @"writer of added commentary" },
            { Role.wal, @"writer of added lyrics" },
            { Role.wam, @"writer of accompanying material" },
            { Role.wat, @"writer of added text" },
            { Role.wdc, @"woodcutter" },
            { Role.wde, @"wood engraver" },
            { Role.win, @"writer of introduction" },
            { Role.wit, @"witness" },
            { Role.wpr, @"writer of preface" },
            { Role.wst, @"writer of supplementary textual content" },
#endregion
        };

        public readonly static IReadOnlyDictionary<Role, string> RoleToFriendlyNameDictionary =
            new ReadOnlyDictionary<Role, string>(_roleToFriendlyNameDictionary);

        private readonly static Dictionary<string, Role> _friendlyNameToRoleDictionary =
            _roleToFriendlyNameDictionary.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

        public readonly static IReadOnlyDictionary<string, Role> FriendlyNameToRoleDictionary =
            new ReadOnlyDictionary<string, Role>(_friendlyNameToRoleDictionary);

        public static string GetFriendlyName(this Role role) => RoleToFriendlyNameDictionary[role];

        public static Role ParseFriendlyName(string friendlyName)
        {
            if (string.IsNullOrWhiteSpace(friendlyName))
            {
                throw new ArgumentNullException(nameof(friendlyName));
            }

            var friendlyNameLower = friendlyName.ToLowerInvariant();
            if (!FriendlyNameToRoleDictionary.ContainsKey(friendlyNameLower))
            {
                throw new ArgumentException("Could not find role matching friendly name: {friendlyNameLower}");
            }

            return FriendlyNameToRoleDictionary[friendlyNameLower];
        }

        public static Role? TryParseFriendlyName(string friendlyName)
        {
            if (string.IsNullOrWhiteSpace(friendlyName))
            {
                throw new ArgumentNullException(nameof(friendlyName));
            }

            Role? role = null;
            try
            {
                role = ParseFriendlyName(friendlyName);
            }
            catch (Exception)
            {
            }

            return role;
        }
    }
}
