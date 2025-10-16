import { useState, useEffect } from 'react';
import {
  Box,
  Button,
  Card,
  CardContent,
  CardActions,
  Typography,
  TextField,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Grid,
  Chip,
  IconButton,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
} from '@mui/material';
import { Add as AddIcon, Delete as DeleteIcon, Visibility as VisibilityIcon } from '@mui/icons-material';
import { templatesApi } from '../services/api';
import type { Template, CreateTemplateDto } from '../types';

interface TemplatesManagerProps {
  tenantId: number;
}

export default function TemplatesManager({ tenantId }: TemplatesManagerProps) {
  const [templates, setTemplates] = useState<Template[]>([]);
  const [showDialog, setShowDialog] = useState(false);
  const [showPreview, setShowPreview] = useState(false);
  const [previewTemplate, setPreviewTemplate] = useState<Template | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  
  const [formData, setFormData] = useState<CreateTemplateDto>({
    tenantId,
    code: '',
    name: '',
    description: '',
    content: '',
    format: 'Html',
  });

  useEffect(() => {
    loadTemplates();
  }, [tenantId]);

  const loadTemplates = async () => {
    setLoading(true);
    try {
      const data = await templatesApi.getByTenant(tenantId);
      setTemplates(data);
      setError('');
    } catch {
      setError('Failed to load templates');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async () => {
    setLoading(true);
    setError('');

    try {
      await templatesApi.create(formData);
      setShowDialog(false);
      setFormData({
        tenantId,
        code: '',
        name: '',
        description: '',
        content: '',
        format: 'Html',
      });
      await loadTemplates();
    } catch {
      setError('Failed to create template');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Are you sure you want to delete this template?')) return;

    try {
      await templatesApi.delete(id);
      await loadTemplates();
    } catch {
      setError('Failed to delete template');
    }
  };

  const handlePreview = (template: Template) => {
    setPreviewTemplate(template);
    setShowPreview(true);
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h5" component="h2">
          Notification Templates
        </Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => setShowDialog(true)}
        >
          Add Template
        </Button>
      </Box>

      {error && (
        <Typography color="error" sx={{ mb: 2 }}>
          {error}
        </Typography>
      )}

      <Grid container spacing={2}>
        {templates.map((template) => (
          <Grid item xs={12} md={6} lg={4} key={template.id}>
            <Card>
              <CardContent>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'start' }}>
                  <Typography variant="h6" component="div">
                    {template.name}
                  </Typography>
                  <Box>
                    <Chip
                      label={template.format}
                      size="small"
                      sx={{ mr: 1 }}
                    />
                    <Chip
                      label={template.isActive ? 'Active' : 'Inactive'}
                      color={template.isActive ? 'success' : 'default'}
                      size="small"
                    />
                  </Box>
                </Box>
                <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                  Code: {template.code}
                </Typography>
                <Typography variant="body2" sx={{ mt: 1 }}>
                  {template.description}
                </Typography>
                <Typography variant="caption" color="text.secondary" sx={{ mt: 1, display: 'block' }}>
                  Created by: {template.createdBy}
                </Typography>
              </CardContent>
              <CardActions>
                <IconButton size="small" color="primary" onClick={() => handlePreview(template)}>
                  <VisibilityIcon />
                </IconButton>
                <IconButton size="small" color="error" onClick={() => handleDelete(template.id)}>
                  <DeleteIcon />
                </IconButton>
              </CardActions>
            </Card>
          </Grid>
        ))}
      </Grid>

      <Dialog open={showDialog} onClose={() => setShowDialog(false)} maxWidth="md" fullWidth>
        <DialogTitle>Create Template</DialogTitle>
        <DialogContent>
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 2 }}>
            <TextField
              label="Code"
              value={formData.code}
              onChange={(e) => setFormData({ ...formData, code: e.target.value })}
              required
              fullWidth
            />
            <TextField
              label="Name"
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              required
              fullWidth
            />
            <TextField
              label="Description"
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              multiline
              rows={2}
              fullWidth
            />
            <FormControl fullWidth>
              <InputLabel>Format</InputLabel>
              <Select
                value={formData.format}
                label="Format"
                onChange={(e) => setFormData({ ...formData, format: e.target.value as 'Html' | 'Markdown' })}
              >
                <MenuItem value="Html">HTML</MenuItem>
                <MenuItem value="Markdown">Markdown</MenuItem>
              </Select>
            </FormControl>
            <TextField
              label="Content"
              value={formData.content}
              onChange={(e) => setFormData({ ...formData, content: e.target.value })}
              multiline
              rows={10}
              required
              fullWidth
              placeholder="Enter your template content here..."
            />
          </Box>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setShowDialog(false)}>Cancel</Button>
          <Button onClick={handleSubmit} variant="contained" disabled={loading}>
            Create
          </Button>
        </DialogActions>
      </Dialog>

      <Dialog open={showPreview} onClose={() => setShowPreview(false)} maxWidth="md" fullWidth>
        <DialogTitle>Template Preview: {previewTemplate?.name}</DialogTitle>
        <DialogContent>
          {previewTemplate && (
            <Box>
              <Typography variant="subtitle2" color="text.secondary" gutterBottom>
                Format: {previewTemplate.format}
              </Typography>
              {previewTemplate.format === 'Html' ? (
                <Box 
                  sx={{ 
                    border: '1px solid #ccc', 
                    borderRadius: 1, 
                    p: 2, 
                    mt: 2,
                    backgroundColor: '#f5f5f5'
                  }}
                  dangerouslySetInnerHTML={{ __html: previewTemplate.content }}
                />
              ) : (
                <Box
                  component="pre"
                  sx={{
                    border: '1px solid #ccc',
                    borderRadius: 1,
                    p: 2,
                    mt: 2,
                    backgroundColor: '#f5f5f5',
                    overflow: 'auto',
                  }}
                >
                  {previewTemplate.content}
                </Box>
              )}
            </Box>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setShowPreview(false)}>Close</Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}
