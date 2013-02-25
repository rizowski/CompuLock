class CreateHistories < ActiveRecord::Migration
  def change
    create_table :histories do |t|
    	t.references :computer

      	t.string :url
      	t.string :title
      	t.integer :visit_count

      	t.timestamps
    end
  end
end
