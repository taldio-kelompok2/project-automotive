function scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
}

window.loginViaFetch = async (url, body) => {
    const res = await fetch(url, {
        method: "POST",
        credentials: "include",
        headers: { "Content-Type": "application/json" },
        body
    });
    
    const data = await res.json();
    return {
        ok: res.ok,
        status: res.status,
        data: data
    };
}

window.logoutViaFetch = async (url, body) => {
    const res = await fetch(url, {
        method: "POST",
        credentials: "include",
        headers: { "Content-Type": "application/json" },
        body
    });
    return res.ok;
}

window.refreshViaFetch = async (url, body) => {
    const res = await fetch(url, {
        method: "POST",
        credentials: "include",
        headers: { "Content-Type": "application/json" },
        body
    });

    await new Promise(r => setTimeout(r, 150));
    return await res.text();
}

window.scrollToTop = () => {
    window.scrollTo({
        top: 0,
        behavior: 'smooth',
        block: 'start',
        inline: 'nearest'
    });
};

