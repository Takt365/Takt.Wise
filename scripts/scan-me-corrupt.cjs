const fs = require('fs');
const path = require('path');
const root = path.resolve(__dirname, '..');
const skip = new Set(['node_modules', 'bin', 'obj', '.git', 'dist', '.vs', 'logs']);
const ext = new Set(['.cs', '.vue', '.ts', '.tsx', '.js', '.cjs', '.json', '.css', '.md', '.mdc']);
const hits = new Map();
function walk(dir) {
  for (const name of fs.readdirSync(dir)) {
    if (skip.has(name)) continue;
    const full = path.join(dir, name);
    const st = fs.statSync(full);
    if (st.isDirectory()) { walk(full); continue; }
    if (!ext.has(path.extname(name).toLowerCase())) continue;
    if (st.size > 2000000) continue;
    const text = fs.readFileSync(full, 'utf8');
    const re = /[A-Za-z]*[Mm]te[a-zA-Z]*/g;
    let m;
    while ((m = re.exec(text))) {
      const token = m[0];
      if (token === 'Mte' || token === 'mte') continue;
      hits.set(token, (hits.get(token) || 0) + 1);
    }
  }
}
walk(root);
const rows = [...hits.entries()].sort((a, b) => b[1] - a[1]);
for (const [token, count] of rows) console.log(count + '\t' + token);
