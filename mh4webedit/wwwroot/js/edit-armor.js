(function(){
    // cache armor name lists (chest/arms/waist/legs/heads) similar to ItemBox caching
    (function(){
        const CACHE_KEY = 'mh4_armor_names_v1';
        try{
            const raw = localStorage.getItem(CACHE_KEY);
            let cached = null;
            if(raw){ cached = JSON.parse(raw); }
            const headers = {};
            if(cached && cached.etag) headers['If-None-Match'] = cached.etag;

            fetch('/api/Options/ArmorNames', { headers: headers })
                .then(function(r){
                    if(r.status === 304) return null;
                    if(!r.ok) throw new Error('Network response not ok');
                    const newEtag = r.headers.get('ETag');
                    return r.json().then(function(names){ return { names: names, etag: newEtag }; });
                })
                .then(function(result){
                    if(result === null){
                        // keep cached if present
                        if(cached && cached.names) populateArmor(cached.names);
                        return;
                    }
                    try{ localStorage.setItem(CACHE_KEY, JSON.stringify({ etag: result.etag, names: result.names })); }catch(e){}
                    populateArmor(result.names);
                })
                .catch(function(err){
                    if(cached && cached.names){ populateArmor(cached.names); }
                    console.error('Failed to load armor names', err);
                });

            function populateArmor(names){
                try{
                    var typeEl = document.getElementById('Type');
                    var sel = document.getElementById('ID');
                    if(!sel || !typeEl) return;
                    var cur = sel.getAttribute('data-current');
                    var t = parseInt(typeEl.value || '1', 10);
                    var arr = names && (t === 1 ? names.chest : t === 2 ? names.arms : t === 3 ? names.waist : t === 4 ? names.legs : t === 5 ? names.heads : names.chest);
                    if(!Array.isArray(arr)) return;
                    sel.innerHTML = arr.map(function(n,i){ return '<option value="'+i+'">'+(n === null ? '' : n.replace(/</g,'&lt;').replace(/>/g,'&gt;'))+'</option>'; }).join('');
                    if(cur !== null && cur !== '' && sel.querySelector('option[value="'+cur+'"]')){ sel.value = cur; }
                }catch(e){ console.error('populateArmor error', e); }
            }
        }catch(e){ console.warn('Armor names cache parse failed', e); }
    })();
    
    // cache and populate jewel options via OptionsController API
    (function(){
        const CACHE_KEY = 'mh4_jewel_options_v1';
        try{
            const raw = localStorage.getItem(CACHE_KEY);
            let cached = null;
            if(raw) cached = JSON.parse(raw);
            const headers = {};
            if(cached && cached.etag) headers['If-None-Match'] = cached.etag;

            fetch('/api/Options/JewelOptions', { headers: headers })
                .then(function(r){
                    if(r.status === 304) return null;
                    if(!r.ok) throw new Error('Network response not ok');
                    const newEtag = r.headers.get('ETag');
                    return r.json().then(function(names){ return { names: names, etag: newEtag }; });
                })
                .then(function(result){
                    if(result === null){
                        if(cached && cached.names) populateJewel(cached.names);
                        return;
                    }
                    try{ localStorage.setItem(CACHE_KEY, JSON.stringify({ etag: result.etag, names: result.names })); }catch(e){}
                    populateJewel(result.names);
                })
                .catch(function(err){
                    if(cached && cached.names) populateJewel(cached.names);
                    console.error('Failed to load jewel options', err);
                });

            function populateJewel(names){
                var optsHtml = names.map(function(x){ return '<option value="'+x.value+'">'+(x.display === null ? '' : x.display.replace(/</g,'&lt;').replace(/>/g,'&gt;'))+'</option>'; }).join('');
                document.querySelectorAll('select.jewel-select').forEach(function(sel){
                    sel.innerHTML = optsHtml;
                    const cur = sel.getAttribute('data-current');
                    if(cur !== null && cur !== '' && sel.querySelector('option[value="'+cur+'"]')){ sel.value = cur; }
                });
            }
        }catch(e){ console.warn('Jewel options cache parse failed', e); }
    })();

    const resTmpl = document.getElementById('resistance-options').innerHTML;
    document.querySelectorAll('select.resistance-select').forEach(function(sel){
        sel.innerHTML = resTmpl;
        const cur = sel.getAttribute('data-current');
        if(cur !== null && cur !== ''){
            const opt = sel.querySelector('option[value="'+cur+'"]');
            if(opt) opt.selected = true;
        }
    });

    const defTmpl = document.getElementById('defense-options').innerHTML;
    document.querySelectorAll('select.defense-select').forEach(function(sel){
        sel.innerHTML = defTmpl;
        const cur = sel.getAttribute('data-current');
        if(cur !== null && cur !== ''){
            const opt = sel.querySelector('option[value="'+cur+'"]');
            if(opt) opt.selected = true;
        }
    });

    const upgTmpl = document.getElementById('upgrade-options').innerHTML;
    document.querySelectorAll('select.upgrade-select').forEach(function(sel){
        sel.innerHTML = upgTmpl;
        const cur = sel.getAttribute('data-current');
        if(cur !== null && cur !== ''){
            const opt = sel.querySelector('option[value="'+cur+'"]');
            if(opt) opt.selected = true;
        }
    });

    const slotsTmpl = document.getElementById('slots-options').innerHTML;
    document.querySelectorAll('select.slots-select').forEach(function(sel){
        sel.innerHTML = slotsTmpl;
        const cur = sel.getAttribute('data-current');
        if(cur !== null && cur !== ''){
            const opt = sel.querySelector('option[value="'+cur+'"]');
            if(opt) opt.selected = true;
        }
    });

    const rarityTmpl = document.getElementById('rarity-options').innerHTML;
    document.querySelectorAll('select.rarity-select').forEach(function(sel){
        sel.innerHTML = rarityTmpl;
        const cur = sel.getAttribute('data-current');
        if(cur !== null && cur !== ''){
            const opt = sel.querySelector('option[value="'+cur+'"]');
            if(opt) opt.selected = true;
        }
    });

    const polishTmpl = document.getElementById('polish-options').innerHTML;
    document.querySelectorAll('select.polish-select').forEach(function(sel){
        sel.innerHTML = polishTmpl;
        const cur = sel.getAttribute('data-current');
        if(cur !== null && cur !== ''){
            const opt = sel.querySelector('option[value="'+cur+'"]');
            if(opt) opt.selected = true;
        }
    });
})();
