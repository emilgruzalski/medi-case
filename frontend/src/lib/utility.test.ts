import { allowedChildTypes, EntityTypes, findNode, findSiblings, formatFileSize, isBlankHtml } from './utility';
import { treeNode } from '../test/factories';

const tree = [treeNode('root', [treeNode('a'), treeNode('b', [treeNode('b1')]), treeNode('c')])];

describe('tree helpers', () => {
  it('finds nested nodes', () => {
    expect(findNode(tree, 'b1')?.id).toBe('b1');
    expect(findNode(tree, 'missing')).toBeUndefined();
  });

  it('finds siblings of a node', () => {
    expect(findSiblings(tree, 'b')?.map((node) => node.id)).toEqual(['a', 'b', 'c']);
    expect(findSiblings(tree, 'root')?.map((node) => node.id)).toEqual(['root']);
  });
});

describe('allowedChildTypes', () => {
  const types = [
    { id: EntityTypes.Category, name: 'Category', canBeRoot: true, allowedChildTypeIds: [EntityTypes.Content] },
    { id: EntityTypes.Content, name: 'Content', canBeRoot: false, allowedChildTypeIds: [] },
  ];

  it('returns root types for the top level', () => {
    expect(allowedChildTypes(types, null)).toEqual([EntityTypes.Category]);
  });

  it('returns children allowed by the parent type', () => {
    expect(allowedChildTypes(types, EntityTypes.Category)).toEqual([EntityTypes.Content]);
    expect(allowedChildTypes(types, EntityTypes.Content)).toEqual([]);
  });
});

describe('formatting', () => {
  it('detects empty editor content', () => {
    expect(isBlankHtml('<p><br></p>')).toBe(true);
    expect(isBlankHtml('<p>ABCDE</p>')).toBe(false);
  });

  it('formats file sizes', () => {
    expect(formatFileSize(2048)).toBe('2 KB');
    expect(formatFileSize(3 * 1024 * 1024)).toBe('3.0 MB');
  });
});
